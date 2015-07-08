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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zumero;

namespace samples
{
    class PresidentsCode : ContentPage
    {

        private DataGrid dg = null;
        public PresidentsCode()
        {
            dg = new DataGrid()
            {
                BackgroundColor = Xamarin.Forms.Color.Black,
                RowHeight = 100,

                FrozenColumn = new Column()
                {
                    Width = 150,
                    Template = new DataTemplate(() =>
                    {
                        var v = new Label
                        {
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center,
                        };
                        v.SetBinding(Label.TextProperty, "full_name");
                        return v;

                    })
                },

                Columns = new List<Column> {
                    new Column() {
                        Width = 200,
                        HeaderView = new Label()
                        {
                            Text = "portrait",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var img = new Image();
                            Binding b = new Binding();
                            b.Converter = new ImageSourceConverter();
                            b.Path = ".";
                            img.SetBinding(Image.SourceProperty, b);
                            return img;
                        })
                    },

                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "inauguration date",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            Binding b = new Binding();
                            b.StringFormat = "{0:d}";
                            b.Path = "inauguration_date";
                            v.SetBinding(Label.TextProperty, b);
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "left office",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            Binding b = new Binding();
                            b.StringFormat = "{0:d}";
                            b.Path = "left_office";
                            v.SetBinding(Label.TextProperty, b);
                            return v;
                        })
                    },

                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "years in office",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "years_in_office");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "age",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "age_at_inauguration");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 140,
                        HeaderView = new Label()
                        {
                            Text = "state",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "state_elected_from");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "occupation",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "Occupation");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "political party",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "political_party");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "college",
                            BackgroundColor = Color.Gray,
                            TextColor = Color.Black,
                            XAlign = TextAlignment.Center,
                            YAlign = TextAlignment.Center
                        },
                        Template = new DataTemplate(() =>
                        {
                            var v = new Label()
                            {
                                BackgroundColor = Color.White,
                                TextColor = Color.Black,
                                XAlign = TextAlignment.Center,
                                YAlign = TextAlignment.Center
                            };
                            v.SetBinding(Label.TextProperty, "College");
                            return v;
                        })
                    },
                }
            };
            
            this.Content = dg;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (dg.Rows == null)
                dg.Rows = await Task.Run(() => {
                    return samples.SampleData.GetPresidents(); 
                });
        }
    }
}
