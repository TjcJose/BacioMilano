using BM.Tools.Web.Captcha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web;

namespace BM.Tools.Web
{
    public static class AdCtrlMvcHelper
    {
        public static MvcHtmlString AdCaptcha(this HtmlHelper helper, string id = AdCaptchaHelper.AdCaptchaId, Style style = Style.Default)
        {
            AdCaptchaOptions opts = AdCaptchaOptions.Instance(style);
            opts.Id = id;
            string value = AdCaptchaHelper.GenerateHiddenValue(opts);
            string content = String.Format(@"<a href='#' title='点击换张图'><img src='/AdCtrl/CaptchaImage?v={0}' alt='验证码'  onclick='$.adCaptcha.refresh({4},{5})' id='{1}' /></a><a href='#' onclick='$.adCaptcha.refresh({4},{5})'>{3}</a><input type='hidden' value='{0}' id='{2}' name='{2}' />", value, opts.ImgCtrlId, opts.HiddenCtrlId, opts.ReloadLinkText, "\"" + opts.Id + "\"", (int)style);
            return MvcHtmlString.Create(content);
        }

        public static MvcHtmlString AdCaptcha_noTxt(this HtmlHelper helper, string id = AdCaptchaHelper.AdCaptchaId, Style style = Style.Default)
        {
            AdCaptchaOptions opts = AdCaptchaOptions.Instance(style);
            opts.Id = id;
            string value = AdCaptchaHelper.GenerateHiddenValue(opts);
            string content = String.Format(@"<a href='#' title='点击换张图'><img src='/AdCtrl/CaptchaImage?v={0}' alt='验证码'  onclick='$.adCaptcha.refresh({4},{5})' id='{1}' /></a><input type='hidden' value='{0}' id='{2}' name='{2}' />", value, opts.ImgCtrlId, opts.HiddenCtrlId, opts.ReloadLinkText, "\"" + opts.Id + "\"", (int)style);
            return MvcHtmlString.Create(content);
        }

        public static MvcHtmlString AdCaptcha_noImgA(this HtmlHelper helper, string imgClass, string linkTxtClass, string linkTxt, string id = AdCaptchaHelper.AdCaptchaId, Style style = Style.Default)
        {
            AdCaptchaOptions opts = AdCaptchaOptions.Instance(style);
            if (!string.IsNullOrWhiteSpace(linkTxt))
            {
                opts.ReloadLinkText = linkTxt;
            }
            opts.Id = id;
            string value = AdCaptchaHelper.GenerateHiddenValue(opts);
            string content = String.Format(@"<img class='{6}' src='/AdCtrl/CaptchaImage?v={0}' alt='验证码'  onclick='$.adCaptcha.refresh({4},{5})' id='{1}' /><a href='#' class='{7}' onclick='$.adCaptcha.refresh({4},{5})'>{3}</a><input type='hidden' value='{0}' id='{2}' name='{2}' />", value, opts.ImgCtrlId, opts.HiddenCtrlId, opts.ReloadLinkText, "\"" + opts.Id + "\"", (int)style, imgClass, linkTxtClass);
            return MvcHtmlString.Create(content);
        }

        public static MvcHtmlString SelectEnum(this HtmlHelper html, Type enumType, string name, bool isAddSpace, string spaceText, int? defaultValue, object htmlAttributes, Func<IEnumerable<BM.Util.EnumItem> , IEnumerable<BM.Util.EnumItem>> funFilter = null)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            if (isAddSpace)
            {
                selectList.Add(new SelectListItem { Text = spaceText, Value = "", Selected = defaultValue == null });
            }

            var items = BM.Util.EnumHelper.GetEnumItems(enumType);
            if (funFilter != null)
            {
                items = funFilter(items);
            }

            foreach (var item in items)
            {
                selectList.Add(new SelectListItem { Text = item.Value, Value = item.Key.ToString(), Selected = defaultValue == item.Key });
            }

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString SelectEnum(this HtmlHelper html, Type enumType, string name, bool isAddSpace, int? defaultValue, object htmlAttributes)
        {
            return SelectEnum(html, enumType, name, isAddSpace, "", defaultValue, htmlAttributes, null);
        }
    }
}
