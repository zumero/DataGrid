/*
   Copyright 2014-2015 Zumero, LLC

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Text;

namespace demo.Models
{

	public class countries : BaseModel
	{
		public countries()
		{
			//Don't fire notfications by default, since
			//they make editing the properties difficult.
			this.NotifyIfPropertiesChange = false;
		}




		public string name 
		{ 
			get { return name_private; }
			set { SetProperty(name_private, value, (val) => { name_private = val; }, name_PropertyName); }
		}
		public static string name_PropertyName = "name";
		private string name_private;
		
		



		public string nativeName 
		{ 
			get { return nativeName_private; }
			set { SetProperty(nativeName_private, value, (val) => { nativeName_private = val; }, nativeName_PropertyName); }
		}
		public static string nativeName_PropertyName = "nativeName";
		private string nativeName_private;
		
		



		public string tld 
		{ 
			get { return tld_private; }
			set { SetProperty(tld_private, value, (val) => { tld_private = val; }, tld_PropertyName); }
		}
		public static string tld_PropertyName = "tld";
		private string tld_private;
		
		



		public string cca2 
		{ 
			get { return cca2_private; }
			set { SetProperty(cca2_private, value, (val) => { cca2_private = val; }, cca2_PropertyName); }
		}
		public static string cca2_PropertyName = "cca2";
		private string cca2_private;
		
		



		public string ccn3 
		{ 
			get { return ccn3_private; }
			set { SetProperty(ccn3_private, value, (val) => { ccn3_private = val; }, ccn3_PropertyName); }
		}
		public static string ccn3_PropertyName = "ccn3";
		private string ccn3_private;
		
		



		public string cca3 
		{ 
			get { return cca3_private; }
			set { SetProperty(cca3_private, value, (val) => { cca3_private = val; }, cca3_PropertyName); }
		}
		public static string cca3_PropertyName = "cca3";
		private string cca3_private;
		
		



		public string currency 
		{ 
			get { return currency_private; }
			set { SetProperty(currency_private, value, (val) => { currency_private = val; }, currency_PropertyName); }
		}
		public static string currency_PropertyName = "currency";
		private string currency_private;
		
		



		public string callingCode 
		{ 
			get { return callingCode_private; }
			set { SetProperty(callingCode_private, value, (val) => { callingCode_private = val; }, callingCode_PropertyName); }
		}
		public static string callingCode_PropertyName = "callingCode";
		private string callingCode_private;
		
		



		public string capital 
		{ 
			get { return capital_private; }
			set { SetProperty(capital_private, value, (val) => { capital_private = val; }, capital_PropertyName); }
		}
		public static string capital_PropertyName = "capital";
		private string capital_private;
		
		



		public string region 
		{ 
			get { return region_private; }
			set { SetProperty(region_private, value, (val) => { region_private = val; }, region_PropertyName); }
		}
		public static string region_PropertyName = "region";
		private string region_private;
		
		



		public string subregion 
		{ 
			get { return subregion_private; }
			set { SetProperty(subregion_private, value, (val) => { subregion_private = val; }, subregion_PropertyName); }
		}
		public static string subregion_PropertyName = "subregion";
		private string subregion_private;
		
		



		public string lang 
		{ 
			get { return lang_private; }
			set { SetProperty(lang_private, value, (val) => { lang_private = val; }, lang_PropertyName); }
		}
		public static string lang_PropertyName = "lang";
		private string lang_private;
		
		



		public string borders 
		{ 
			get { return borders_private; }
			set { SetProperty(borders_private, value, (val) => { borders_private = val; }, borders_PropertyName); }
		}
		public static string borders_PropertyName = "borders";
		private string borders_private;
		
		



		public Nullable<double> area 
		{ 
			get { return area_private; }
			set { SetProperty(area_private, value, (val) => { area_private = val; }, area_PropertyName); }
		}
		public static string area_PropertyName = "area";
		private Nullable<double> area_private;
		
		

		public int id 
		{ 
			get { return id_private; }
			set { SetProperty(id_private, value, (val) => { id_private = val; }, id_PropertyName); }
		}
		public static string id_PropertyName = "id";
		private int id_private;
		
		


		public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();

			if (name != null)
			{
				sb.Append(name.ToString());
			}
			sb.Append("|");

			if (nativeName != null)
			{
				sb.Append(nativeName.ToString());
			}
			sb.Append("|");

			if (tld != null)
			{
				sb.Append(tld.ToString());
			}
			sb.Append("|");

			if (cca2 != null)
			{
				sb.Append(cca2.ToString());
			}
			sb.Append("|");

			if (ccn3 != null)
			{
				sb.Append(ccn3.ToString());
			}
			sb.Append("|");

			if (cca3 != null)
			{
				sb.Append(cca3.ToString());
			}
			sb.Append("|");

			if (currency != null)
			{
				sb.Append(currency.ToString());
			}
			sb.Append("|");

			if (callingCode != null)
			{
				sb.Append(callingCode.ToString());
			}
			sb.Append("|");

			if (capital != null)
			{
				sb.Append(capital.ToString());
			}
			sb.Append("|");

			if (region != null)
			{
				sb.Append(region.ToString());
			}
			sb.Append("|");

			if (subregion != null)
			{
				sb.Append(subregion.ToString());
			}
			sb.Append("|");

			if (lang != null)
			{
				sb.Append(lang.ToString());
			}
			sb.Append("|");

			if (borders != null)
			{
				sb.Append(borders.ToString());
			}
			sb.Append("|");

			if (area.HasValue)
			{
				sb.Append(area.ToString());
			}
			sb.Append("|");

			sb.Append(id.ToString());

			sb.Append("|");

			return sb.ToString();
		}
	}
}
