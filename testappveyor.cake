if (BuildSystem.IsRunningOnAppVeyor)
{

    Zip("./", "./build.sh.zip", "build.sh");
    Zip("./", "./build.zip", "build.cake");
    var nugetPackages = GetFiles("./artifacts/**/*.nupkg");

    BuildSystem.AppVeyor.UploadArtifact("./build.sh.zip", new AppVeyorUploadArtifactsSettings {
        ArtifactType = AppVeyorUploadArtifactType.Auto
        });

    BuildSystem.AppVeyor.UploadArtifact("./build.zip", new AppVeyorUploadArtifactsSettings {
            ArtifactType = AppVeyorUploadArtifactType.WebDeployPackage
        });

    var odd = false;
    foreach(var nuget in nugetPackages)
    {
        BuildSystem.AppVeyor.UploadArtifact(nuget,
            settings=>settings
            .SetArtifactType((odd) ? AppVeyorUploadArtifactType.Auto : AppVeyorUploadArtifactType.NuGetPackage)
            .SetDeploymentName(odd ? "Odd" : "Even"));
        odd = !odd;
    }

    //BuildSystem.AppVeyor.UploadTestResults("./TestResults.xml", AppVeyorTestResultsType.XUnit);
}