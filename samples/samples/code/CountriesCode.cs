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
    class CountriesCode : ContentPage
    {

        private DataGrid dg = null;
        public CountriesCode()
        {
            dg = new DataGrid()
            {
                BackgroundColor = Xamarin.Forms.Color.Black,
                RowHeight = 80,

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
                        v.SetBinding(Label.TextProperty, "name");
                        return v;

                    })
                },

                Columns = new List<Column> {
                    new Column() {
                        Width = 200,
                        HeaderView = new Label()
                        {
                            Text = "native name",
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
                            v.SetBinding(Label.TextProperty, "nativeName");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "tld",
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
                            v.SetBinding(Label.TextProperty, "tld");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "cca2",
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
                            v.SetBinding(Label.TextProperty, "cca2");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "ccn3",
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
                            v.SetBinding(Label.TextProperty, "ccn3");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 50,
                        HeaderView = new Label()
                        {
                            Text = "cca3",
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
                            v.SetBinding(Label.TextProperty, "cca3");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 80,
                        HeaderView = new Label()
                        {
                            Text = "currency",
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
                            v.SetBinding(Label.TextProperty, "currency");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 60,
                        HeaderView = new Label()
                        {
                            Text = "calling code",
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
                            v.SetBinding(Label.TextProperty, "callingCode");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "capital",
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
                            v.SetBinding(Label.TextProperty, "capital");
                            return v;
                        })
                    },
                     
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "region",
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
                            v.SetBinding(Label.TextProperty, "region");
                            return v;
                        })
                    },

                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "subregion",
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
                            v.SetBinding(Label.TextProperty, "subregion");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 130,
                        HeaderView = new Label()
                        {
                            Text = "lang",
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
                            v.SetBinding(Label.TextProperty, "lang");
                            return v;
                        })
                    },
                    
                    new Column() {
                        Width = 150,
                        HeaderView = new Label()
                        {
                            Text = "borders",
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
                            v.SetBinding(Label.TextProperty, "borders");
                            return v;
                        })
                    },
                                        
                    new Column() {
                        Width = 100,
                        HeaderView = new Label()
                        {
                            Text = "area",
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
                            v.SetBinding(Label.TextProperty, "area");
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
                    return samples.SampleData.GetCountries(); 
                });
        }
    }
}
