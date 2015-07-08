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
	public class presidents : BaseModel
	{
		public presidents()
		{
			//Don't fire notfications by default, since
			//they make editing the properties difficult.
			this.NotifyIfPropertiesChange = false;
		}


		public int id 
		{ 
			get { return id_private; }
			set { SetProperty(id_private, value, (val) => { id_private = val; }, id_PropertyName); }
		}
		public static string id_PropertyName = "id";
		private int id_private;
		
		



		public string full_name 
		{ 
			get { return full_name_private; }
			set { SetProperty(full_name_private, value, (val) => { full_name_private = val; }, full_name_PropertyName); }
		}
		public static string full_name_PropertyName = "full_name";
		private string full_name_private;
		
		



		public Nullable<double> years_in_office 
		{ 
			get { return years_in_office_private; }
			set { SetProperty(years_in_office_private, value, (val) => { years_in_office_private = val; }, years_in_office_PropertyName); }
		}
		public static string years_in_office_PropertyName = "years_in_office";
		private Nullable<double> years_in_office_private;
		
		




		// The actual column definition, as seen in SQLite
		public string inauguration_date_raw { get; set; }

		public static string inauguration_date_PropertyName = "inauguration_date";
		
		// A helper definition that will not be saved to SQLite directly.
		// This property reads and writes to the _raw property.
		public Nullable<DateTime> inauguration_date { 
			// Watch out for time zones, as they are not encoded into
			// the database. Here, I make no assumptions about time
			// zones.
			get { return inauguration_date_raw != null ? (Nullable<DateTime>)DateTime.Parse(inauguration_date_raw) : (Nullable<DateTime>)null; }
			set { SetProperty(inauguration_date_raw, inauguration_date_ConvertToString(value), (val) => { inauguration_date_raw = val; }, inauguration_date_PropertyName); }
		}

		// This static method is helpful when you need to query
		// on the raw value.
		public static string inauguration_date_ConvertToString(Nullable<DateTime> date)
		{
			if (!date.HasValue)
				return null;
			else
	
			return date.Value.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
		
		}

		




		// The actual column definition, as seen in SQLite
		public string left_office_raw { get; set; }

		public static string left_office_PropertyName = "left_office";
		
		// A helper definition that will not be saved to SQLite directly.
		// This property reads and writes to the _raw property.
		public Nullable<DateTime> left_office { 
			// Watch out for time zones, as they are not encoded into
			// the database. Here, I make no assumptions about time
			// zones.
			get { return left_office_raw != null ? (Nullable<DateTime>)DateTime.Parse(left_office_raw) : (Nullable<DateTime>)null; }
			set { SetProperty(left_office_raw, left_office_ConvertToString(value), (val) => { left_office_raw = val; }, left_office_PropertyName); }
		}

		// This static method is helpful when you need to query
		// on the raw value.
		public static string left_office_ConvertToString(Nullable<DateTime> date)
		{
			if (!date.HasValue)
				return null;
			else
	
			return date.Value.ToString("yyyy-MM-dd HH:mm:ss.fffffff");
		
		}

		



		public Nullable<int> age_at_inauguration 
		{ 
			get { return age_at_inauguration_private; }
			set { SetProperty(age_at_inauguration_private, value, (val) => { age_at_inauguration_private = val; }, age_at_inauguration_PropertyName); }
		}
		public static string age_at_inauguration_PropertyName = "age_at_inauguration";
		private Nullable<int> age_at_inauguration_private;
		
		



		public string state_elected_from 
		{ 
			get { return state_elected_from_private; }
			set { SetProperty(state_elected_from_private, value, (val) => { state_elected_from_private = val; }, state_elected_from_PropertyName); }
		}
		public static string state_elected_from_PropertyName = "state_elected_from";
		private string state_elected_from_private;
		
		



		public Nullable<int> electoral_votes 
		{ 
			get { return electoral_votes_private; }
			set { SetProperty(electoral_votes_private, value, (val) => { electoral_votes_private = val; }, electoral_votes_PropertyName); }
		}
		public static string electoral_votes_PropertyName = "electoral_votes";
		private Nullable<int> electoral_votes_private;
		
		



		public Nullable<int> total_electoral_votes 
		{ 
			get { return total_electoral_votes_private; }
			set { SetProperty(total_electoral_votes_private, value, (val) => { total_electoral_votes_private = val; }, total_electoral_votes_PropertyName); }
		}
		public static string total_electoral_votes_PropertyName = "total_electoral_votes";
		private Nullable<int> total_electoral_votes_private;
		
		



		public Nullable<double> percent_electoral 
		{ 
			get { return percent_electoral_private; }
			set { SetProperty(percent_electoral_private, value, (val) => { percent_electoral_private = val; }, percent_electoral_PropertyName); }
		}
		public static string percent_electoral_PropertyName = "percent_electoral";
		private Nullable<double> percent_electoral_private;
		
		



		public Nullable<int> popular_votes 
		{ 
			get { return popular_votes_private; }
			set { SetProperty(popular_votes_private, value, (val) => { popular_votes_private = val; }, popular_votes_PropertyName); }
		}
		public static string popular_votes_PropertyName = "popular_votes";
		private Nullable<int> popular_votes_private;
		
		



		public Nullable<int> total_national_votes 
		{ 
			get { return total_national_votes_private; }
			set { SetProperty(total_national_votes_private, value, (val) => { total_national_votes_private = val; }, total_national_votes_PropertyName); }
		}
		public static string total_national_votes_PropertyName = "total_national_votes";
		private Nullable<int> total_national_votes_private;
		
		



		public Nullable<double> percent_popular 
		{ 
			get { return percent_popular_private; }
			set { SetProperty(percent_popular_private, value, (val) => { percent_popular_private = val; }, percent_popular_PropertyName); }
		}
		public static string percent_popular_PropertyName = "percent_popular";
		private Nullable<double> percent_popular_private;
		
		



		public string political_party 
		{ 
			get { return political_party_private; }
			set { SetProperty(political_party_private, value, (val) => { political_party_private = val; }, political_party_PropertyName); }
		}
		public static string political_party_PropertyName = "political_party";
		private string political_party_private;
		
		



		public string Occupation 
		{ 
			get { return Occupation_private; }
			set { SetProperty(Occupation_private, value, (val) => { Occupation_private = val; }, Occupation_PropertyName); }
		}
		public static string Occupation_PropertyName = "Occupation";
		private string Occupation_private;
		
		



		public string College 
		{ 
			get { return College_private; }
			set { SetProperty(College_private, value, (val) => { College_private = val; }, College_PropertyName); }
		}
		public static string College_PropertyName = "College";
		private string College_private;
		
		



		public byte[] portrait 
		{ 
			get { return portrait_private; }
			set { SetProperty(portrait_private, value, (val) => { portrait_private = val; }, portrait_PropertyName); }
		}
		public static string portrait_PropertyName = "portrait";
		private byte[] portrait_private;
		
		


		public override string ToString() 
		{
			StringBuilder sb = new StringBuilder();

			sb.Append(id.ToString());

			sb.Append("|");

			if (full_name != null)
			{
				sb.Append(full_name.ToString());
			}
			sb.Append("|");

			if (years_in_office.HasValue)
			{
				sb.Append(years_in_office.ToString());
			}
			sb.Append("|");

			if (inauguration_date != null)
			{
				sb.Append(inauguration_date_ConvertToString(inauguration_date));
			}
			sb.Append("|");

			if (left_office != null)
			{
				sb.Append(left_office_ConvertToString(left_office));
			}
			sb.Append("|");

			if (age_at_inauguration.HasValue)
			{
				sb.Append(age_at_inauguration.ToString());
			}
			sb.Append("|");

			if (state_elected_from != null)
			{
				sb.Append(state_elected_from.ToString());
			}
			sb.Append("|");

			if (electoral_votes.HasValue)
			{
				sb.Append(electoral_votes.ToString());
			}
			sb.Append("|");

			if (total_electoral_votes.HasValue)
			{
				sb.Append(total_electoral_votes.ToString());
			}
			sb.Append("|");

			if (percent_electoral.HasValue)
			{
				sb.Append(percent_electoral.ToString());
			}
			sb.Append("|");

			if (popular_votes.HasValue)
			{
				sb.Append(popular_votes.ToString());
			}
			sb.Append("|");

			if (total_national_votes.HasValue)
			{
				sb.Append(total_national_votes.ToString());
			}
			sb.Append("|");

			if (percent_popular.HasValue)
			{
				sb.Append(percent_popular.ToString());
			}
			sb.Append("|");

			if (political_party != null)
			{
				sb.Append(political_party.ToString());
			}
			sb.Append("|");

			if (Occupation != null)
			{
				sb.Append(Occupation.ToString());
			}
			sb.Append("|");

			if (College != null)
			{
				sb.Append(College.ToString());
			}
			sb.Append("|");

			if (portrait != null)
			{
				sb.Append(portrait.ToString());
			}
			sb.Append("|");

			return sb.ToString();
		}
	}
}
