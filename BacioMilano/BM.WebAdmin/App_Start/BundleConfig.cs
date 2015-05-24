using System.Web;
using System.Web.Optimization;

namespace BM.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                       "~/Scripts/jquery.validate.js",
                       "~/Scripts/jquery.validate.unobtrusive.js",
                       "~/Scripts/jquery.validate.ext.js",
                       "~/Scripts/jquery.form.js"
                       ));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-paginator.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                     "~/Scripts/dot.min.js",
                     "~/Scripts/jquery.blockUI.js",
                     "~/Scripts/ad_string.js",
                     "~/Scripts/ad_select.js",
                      "~/Scripts/ad.js",
                      "~/Scripts/ad_captcha.js",
                      "~/Scripts/ad_form.js"));

            bundles.Add(new ScriptBundle("~/bundles/respond").Include(
                     "~/Scripts/respond.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/bootstrap.ext.css",
                      "~/Content/site.css"));
        }
    }
}

