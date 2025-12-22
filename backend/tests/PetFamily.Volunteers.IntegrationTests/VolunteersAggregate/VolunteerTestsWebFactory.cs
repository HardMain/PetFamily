using Core.Providers;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using SharedKernel.Failures;
using SharedKernel.ValueObjects;

namespace PetFamily.Volunteers.IntegrationTests.VolunteersAggregate
{
    public class VolunteerTestsWebFactory : IntegrationTestsWebFactory
    {
        private readonly IFilesProvider _fileProviderMock = Substitute.For<IFilesProvider>();

        public VolunteerTestsWebFactory() { }

        protected override void ConfigureDefaultServices(IServiceCollection services)
        {
            base.ConfigureDefaultServices(services);

            var fileProvider = services
                .SingleOrDefault(s => s.ServiceType == typeof(IFilesProvider));

            if (fileProvider is not null)
                services.Remove(fileProvider);

            services.AddSingleton(_fileProviderMock);
        }

        public void SetupSuccessFileProviderMock()
        {
            _fileProviderMock
                .UploadFiles(Arg.Any<IEnumerable<FileStorageUploadDto>>(), Arg.Any<CancellationToken>())
                .Returns(
                Task.FromResult(
                    Result<IReadOnlyList<string>, ErrorList>
                    .Success(new List<string> { "path1.jpg", "path2.jpg" })));

            _fileProviderMock
                .DeleteFiles(Arg.Any<IEnumerable<FileStorageDeleteDto>>(), Arg.Any<CancellationToken>())
                .Returns(
                Task.FromResult(
                    Result<IReadOnlyList<string>, ErrorList>
                    .Success(new List<string> { "path3.jpg", "path4.jpg" })));
        }

        public void SetupFailureFileProviderMock()
        {
            _fileProviderMock
                .UploadFiles(Arg.Any<IEnumerable<FileStorageUploadDto>>(), Arg.Any<CancellationToken>())
                .Returns(
                Task.FromResult(
                    Result<IReadOnlyList<string>, ErrorList>
                    .Failure(Errors.MinioProvider.FileUploadError())));

            _fileProviderMock
                .DeleteFiles(Arg.Any<IEnumerable<FileStorageDeleteDto>>(), Arg.Any<CancellationToken>())
                .Returns(
                Task.FromResult(
                    Result<IReadOnlyList<string>, ErrorList>
                    .Failure(Errors.MinioProvider.FileDeleteError())));
        }
    }
}
