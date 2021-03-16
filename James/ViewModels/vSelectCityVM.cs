using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace James.ViewModels
{
    public class SelectViewModel
    { /// <summary> /// select標籤Id /// </summary> public String Id { get; set; } 
      /// <summary> /// select標籤Name /// </summary> public String Name { get; set; }
      /// /// <summary> /// json資料來源元素中代表select>option>Text的屬性 
      /// /// </summary> public String TextProperty { get; set; } /// <summary>
      /// /// json資料來源元素中代表select>option>value的屬性 
      /// /// </summary> public String ValueProperty { get; set; } 
      /// /// <summary> /// 資料來源獲取地址 ///
      /// </summary> public String ActionUrl { get; set; } /// <summary> /// 
      /// select標籤初始項文字,預設為空字串 /// </summary> 
      /// public String DefaultText { get; set; } /// <summary> /// 
      /// select標籤初始項值,預設為空字串 /// 
      /// </summary> public String DefaultValue{ get; set; } /// <summary> ///
      /// 獲取資料時傳遞的引數名 ///
      /// </summary> public String ParamName { get; set; } /// <summary> /// 
      /// 父級下拉框的標籤id /// 有父級必選 /// </summary> 
      /// public String ParentTagId { get; set; } /// <summary> /// 
      /// 樣式表 /// </summary> public String Class { get; set; } /// <summary> ///
      /// 樣式 /// </summary> public String Style { get; set; } /// <summary> ///
      /// select標籤當前選定項 /// </summary> public String SelectedValue { get; set; } 
      /// private FormMethod requestMethod = FormMethod.Get; /// <summary> /// 
      /// 請求方式 /// 預設為GET /// </summary>
      /// public FormMethod RequestMethod { get { return requestMethod; } set { requestMethod = value; } } }}
      /// }
      /// 

        public int cityId { get; set; }
        public string province { get; set; }
        public String TextProperty { get; set; }
        public String ValueProperty { get; set; }
        public String ActionUrl { get; set; }
        public String DefaultText { get; set; }
        public String DefaultValue { get; set; }
        public String ParamName { get; set; }
        public String ParentTagId { get; set; }
        public String Class { get; set; }
        public String Style { get; set; }
        public String SelectedValue { get; set; }

        private FormMethod requestMethod = FormMethod.Get;
        public FormMethod RequestMethod { get { return requestMethod; } set { requestMethod = value; } }
    }
}
