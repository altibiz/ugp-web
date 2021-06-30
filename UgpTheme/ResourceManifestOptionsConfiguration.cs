using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Themes.UgpTheme
{
    public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
    {
        private static ResourceManifest _manifest;

        static ResourceManagementOptionsConfiguration()
        {
            _manifest = new ResourceManifest();

            _manifest
                .DefineScript("ugptheme-bootstrap-bundle")
                .SetCdn("https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.min.js", "https://cdn.jsdelivr.net/npm/bootstrap@5.0.1/dist/js/bootstrap.bundle.js")
                .SetCdnIntegrity("sha384-gtEjrD/SeCtmISkJkNUaaKMoLD0//ElJ19smozuHV6z3Iehds+3Ulb9Bn9Plx0x4", "sha384-zlQmapo6noJSGz1A/oxylOFtN0k8EiXX45sOWv3x9f/RGYG0ECMxTbMao6+OLt2e")
                .SetVersion("5.0.1");

            _manifest
                .DefineScript("ugptheme-jQuery")
                .SetCdn("https://code.jquery.com/jquery-3.4.1.min.js", "https://code.jquery.com/jquery-3.4.1.js")
                .SetCdnIntegrity("sha384-vk5WoKIaW/vJyUAd9n/wmopsmNhiy+L2Z+SBxGYnUkunIxVxAv/UtMOhba/xskxh", "sha384-mlceH9HlqLp7GMKHrj5Ara1+LvdTZVMx4S1U43/NxCvAkzIo8WJ0FE7duLel3wVo")
                .SetVersion("3.4.1");

            _manifest
                .DefineStyle("ugptheme-bootstrap-oc")
                .SetUrl("~/UgpTheme/css/bootstrap-oc.min.css", "~/UgpTheme/css/bootstrap-oc.css")
                .SetVersion("1.0.0");

            _manifest
                .DefineScript("ugptheme")
                .SetDependencies("ugptheme-bootstrap-bundle")
                .SetUrl("~/UgpTheme/js/scripts.min.js", "~/UgpTheme/js/scripts.js")
                .SetVersion("6.0.0");

            _manifest
                .DefineStyle("ugptheme")
                .SetUrl("~/UgpTheme/css/styles.min.css", "~/UgpTheme/css/styles.css")
                .SetVersion("6.0.0");
        }

        public void Configure(ResourceManagementOptions options)
        {
            options.ResourceManifests.Add(_manifest);
        }
    }
}
