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
                .SetCdn("https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js", "https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.js")
                .SetCdnIntegrity("sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p", "sha384-8fq7CZc5BnER+jVlJI2Jafpbn4A9320TKhNJfYP33nneHep7sUg/OD30x7fK09pS")
                .SetVersion("5.1.3");

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
                .DefineScript("ugptheme-custom")
                .SetDependencies("ugptheme-jQuery")
                .SetUrl("~/UgpTheme/js/custom.js")
                .SetVersion("6.0.0");

            _manifest
                .DefineScript("ugptheme-libbcmath")
                .SetUrl("~/UgpTheme/js/libbcmath.js");
            _manifest
                .DefineScript("ugptheme-bcmath")
                .SetUrl("~/UgpTheme/js/bcmath.js");
            _manifest
                .DefineScript("ugptheme-pdf417")
                .SetUrl("~/UgpTheme/js/pdf417.js")
                .SetVersion("1.0.005");

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
