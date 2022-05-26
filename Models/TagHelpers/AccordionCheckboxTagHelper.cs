using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ToDoManage.Models.Data;

namespace ToDoManage.Models.TagHelpers
{
    public class AccordionCheckboxTagHelper : TagHelper
    {
        [HtmlAttributeName("model")]
        public ToDoTask task { get; set; }
        [HtmlAttributeName("number")]
        public int number { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // タグをDivに変更
            output.TagName = "div";

            // クラスの追加
            output.AddClass("accordion-item", HtmlEncoder.Default);

            var content = output.Content;

            // ヘッダーの作成
            var header = new TagBuilder("h2");
            header.AddCssClass("accordion-header");
            header.Attributes.Add("id", $"accordionHeader_{number}");

            // チェックボックスの作成
            var checkbox = new TagBuilder("input");
            checkbox.AddCssClass("form-check-input");
            checkbox.Attributes.Add("type", "checkbox");
            header.InnerHtml.AppendHtml(checkbox);

            // ボタンの作成
            var accordionButton = new TagBuilder("button");
            accordionButton.AddCssClass("accordion-button");
            accordionButton.Attributes.Add("type", "button");
            accordionButton.Attributes.Add("data-bs-toggle", "collapse");
            accordionButton.Attributes.Add("data-bs-target", $"#accordionCollapse_{number}");
            accordionButton.Attributes.Add("area-expanded", "true");
            accordionButton.Attributes.Add("area-controls", $"accordionCollapse_{number}");
            accordionButton.InnerHtml.Append($"{task.title}");
            header.InnerHtml.AppendHtml(accordionButton);

            content.SetHtmlContent(header);

            // 開閉する部分の作成
            var collapseDiv = new TagBuilder("div");
            collapseDiv.AddCssClass("accordion-collapse collapse");
            collapseDiv.Attributes.Add("id", $"accordionCollapse_{number}");
            collapseDiv.Attributes.Add("aria-labelledby", $"accordionHeader_{number}");
            collapseDiv.Attributes.Add("data-bs-parent", $"#accordionCheckBox");
            
            // コンテンツ表示部分の作成
            var bodyDiv = new TagBuilder("div");
            bodyDiv.AddCssClass("accordion-body");
            bodyDiv.InnerHtml.Append($"{task.description}");
            collapseDiv.InnerHtml.AppendHtml(bodyDiv);

            content.AppendHtml(collapseDiv);

        }
    }
}
