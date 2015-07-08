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
using System.Collections.ObjectModel;

using Xamarin.Forms;

using Zumero;

namespace demo
{
	public static class testData
	{
		public static IList<Column> createTwoSimpleColumns()
		{
			return new ObservableCollection<Column> {
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "English",
						BackgroundColor = Color.Gray,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					},
					Template = new DataTemplate (() => {
						var v = new Label {
							BackgroundColor = Color.White,
							TextColor = Color.Black,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						};
						v.SetBinding (Label.TextProperty, "English");
						return v;
					}),
				},
				new Column {
					Width = 100,
					HeaderView = new Label {
						Text = "Spanish",
						BackgroundColor = Color.Gray,
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center,
					},
					Template = new DataTemplate (() => {
						var v = new Label {
							BackgroundColor = Color.White,
							TextColor = Color.Black,
							XAlign = TextAlignment.Center,
							YAlign = TextAlignment.Center,
						};
						v.SetBinding (Label.TextProperty, "Spanish");
						return v;
					}),
				},
			};
		}

		public static IList<object> createFourSimpleWordPairs()
		{
			return new ObservableCollection<object> {
				new WordPair { 
					English = "speak", 
					Spanish = "hablar", 
				},
				new WordPair { 
					English = "ask", 
					Spanish = "preguntar",
				},
				new WordPair { 
					English = "raise", 
					Spanish = "levantar",
				},
				new WordPair { 
					English = "fall", 
					Spanish = "caer", 
				},
			};
		}

		public static IList<object> createLotsOfWordPairsWithButtons(DataGrid dg)
		{
			return new ObservableCollection<object> {
				new WordPair { 
					English = "speak", 
					Spanish = "hablar", 
					ButtonText = "RowHeight+", 
					X = 31,
					Action = () => {
						var a = new Animation ((x) => {
							dg.RowHeight = x;
						}, dg.RowHeight, dg.RowHeight * 1.2, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "ask", 
					Spanish = "preguntar" ,
					ImageName = "breaking.png",
					ButtonText = "RowHeight-", 
					X = 27,
					Action = () => {
						var a = new Animation ((x) => {
							dg.RowHeight = x;
						}, dg.RowHeight, dg.RowHeight * 0.8, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "raise", 
					Spanish = "levantar" ,
					ButtonText = "RowSpacing+", 
					X = 31,
					Action = () => {
						var a = new Animation ((x) => {
							dg.RowSpacing = x;
						}, dg.RowSpacing, dg.RowSpacing * 2, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "fall", 
					Spanish = "caer", 
					ImageName = "breaking.png",
					ButtonText = "RowSpacing-", 
					X = 27,
					Action = () => {
						var a = new Animation ((x) => {
							dg.RowSpacing = x;
						}, dg.RowSpacing, dg.RowSpacing / 2, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "live", 
					Spanish = "vivir" ,
					X = 11,
					ButtonText = "R.English.append *", 
					Action = () => {
						WordPair me = dg.Rows[2] as WordPair;
						me.English += "*";
					}
				},
				new WordPair { 
					English = "go", 
					Spanish = "ir", 
					SpanishBackgroundColor = Color.Yellow,
					ButtonText = "C[0].Width+", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.Columns[0].Width = x;
						}, dg.Columns[0].Width, dg.Columns[0].Width * 1.2, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "be", 
					Spanish = "ser" ,
					ButtonText = "C[-1].Width+", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.FrozenColumn.Width = x;
						}, dg.FrozenColumn.Width, dg.FrozenColumn.Width * 1.5, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "run", 
					Spanish = "correr" ,
					ButtonText = "C[-1].Width-", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.FrozenColumn.Width = x;
						}, dg.FrozenColumn.Width, dg.FrozenColumn.Width * 0.8, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "give", 
					Spanish = "dar" ,
					ButtonText = "ColumnSpacing+", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.ColumnSpacing = x;
						}, dg.ColumnSpacing, dg.ColumnSpacing * 2, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "lose", 
					Spanish = "perder" ,
					ButtonText = "ColumnSpacing-", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.ColumnSpacing = x;
						}, dg.ColumnSpacing, dg.ColumnSpacing * 0.5, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "walk", 
					Spanish = "andar" ,
					ButtonText = "C append", 
					Action = () => {
						dg.Columns.Add(new Column {
							Width = 90,
							HeaderView = new BoxView {Color = Color.Red},
							Template = new DataTemplate (() => {
								var v = new Label {
									BackgroundColor = Color.Gray,
									TextColor = Color.Black,
									XAlign = TextAlignment.Center,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding(Label.TextProperty, "English");
								return v;
							}),
						});
					}
				},
				new WordPair { 
					English = "jump", 
					Spanish = "saltar" ,
					ButtonText = "C[last] -", 
					Action = () => {
						dg.Columns.RemoveAt(dg.Columns.Count-1);
					},
				},
				new WordPair { 
					English = "say", 
					Spanish = "decir" ,
					ButtonText = "C[1] -", 
					Action = () => {
						dg.Columns.RemoveAt(1);
					}
				},
				new WordPair { 
					English = "make", 
					Spanish = "hacer", 
					SpanishBackgroundColor = Color.Yellow  ,
					ButtonText = "R insert(5)", 
					Action = () => {
						dg.Rows.Insert(5, new WordPair {English = "love", Spanish="amar"});
					}
				},
				new WordPair { 
					English = "read", 
					Spanish = "leer", 
					ButtonText = "R append", 
					Action = () => {
						dg.Rows.Add(new WordPair {English = "see", Spanish="ver"});
					}
				},
				new WordPair { 
					English = "sleep", 
					Spanish = "dormir" ,
					ButtonText = "R[7] rm", 
					Action = () => {
						dg.Rows.RemoveAt(7);
					},
				},
				new WordPair { 
					English = "work", 
					Spanish = "trabajar" ,
					ButtonText = "R[last] rm", 
					Action = () => {
						dg.Rows.RemoveAt(dg.Rows.Count-1);
					},
				},
				new WordPair { 
					English = "drink", 
					Spanish = "beber" ,
					ButtonText = "R[6] replace", 
					Action = () => {
						dg.Rows[6] = new WordPair { English="I am milk", Spanish="Soy milk" };
					},
				},
				new WordPair { 
					English = "think", 
					Spanish = "pensar" ,
					ButtonText = "FrozRow.Height+", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.HeaderHeight = x;
						}, dg.HeaderHeight, dg.HeaderHeight * 1.5, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "like", 
					Spanish = "querer" ,
					ButtonText = "FrozRow.Height-", 
					Action = () => {
						var a = new Animation ((x) => {
							dg.HeaderHeight = x;
						}, dg.HeaderHeight, dg.HeaderHeight * 0.8, null, null);
						dg.Animate ("foo", a, 16, 1000);
					}
				},
				new WordPair { 
					English = "grow", 
					Spanish = "crecer" ,
				},
				new WordPair { 
					English = "kill", 
					Spanish = "matar" ,
				},
				new WordPair { 
					English = "throw", 
					Spanish = "tirar" ,
					ButtonText = "C[2] HeaderView", 
					Action = () => {
						dg.Columns[2].HeaderView = new DatePicker();
					}
				},
				new WordPair { 
					English = "please", 
					Spanish = "gustar" ,
				},
				new WordPair { 
					English = "sing", 
					Spanish = "cantar" ,
				},
				new WordPair { 
					English = "have", 
					Spanish = "tener" ,
					ButtonText = "FrozRow Aqua", 
					Action = () => {
						foreach (var c in dg.Columns) {
							if (c.HeaderView != null) {
								c.HeaderView.BackgroundColor = Color.Aqua;
							}
						}
					}
				},
				new WordPair { 
					English = "write", 
					Spanish = "escribir" ,
					ButtonText = "C[0] Template", 
					Action = () => {
						dg.Columns[0].Template = new DataTemplate (() => {
							var v = new Label {
								BackgroundColor = Color.Maroon,
								TextColor = Color.Black,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							};
							v.SetBinding(Label.TextProperty, "English");
							return v;
						});
					}
				},
				new WordPair { 
					English = "know", 
					Spanish = "conocer" ,
					ButtonText = "C[0] replace", 
					Action = () => {
						var oldc = dg.Columns[0];
						dg.Columns[0] = new Column {
							Width = 140,
							HeaderView = new Label {
								Text = "Ingles",
								BackgroundColor = Color.Purple,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							},
							Template = new DataTemplate (() => {
								var v = new Label {
									BackgroundColor = Color.Pink,
									TextColor = Color.Black,
									XAlign = TextAlignment.Center,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding(Label.TextProperty, "English");
								return v;
							}),
						};
						oldc.Width = 300;
					},
				},
				new WordPair { 
					English = "come", 
					Spanish = "venir" ,
					ButtonText = "C insert(2)", 
					Action = () => {
						dg.Columns.Insert(2, new Column {
							Width = 140,
							HeaderView = new Label {
								Text = "Ingles",
								BackgroundColor = Color.Purple,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							},
							Template = new DataTemplate (() => {
								var v = new Label {
									BackgroundColor = Color.Pink,
									TextColor = Color.Black,
									XAlign = TextAlignment.Center,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding(Label.TextProperty, "English");
								return v;
							}),
						}
						);
					}
				},
				new WordPair { 
					English = "eat", 
					Spanish = "comer" ,
					ButtonText = "C[-1] replace", 
					Action = () => {
						var oldc = dg.FrozenColumn;
						dg.FrozenColumn = new Column {
							Width = 140,
							HeaderView = new Label {
								Text = "CRNR",
								BackgroundColor = Color.Purple,
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center,
							},
							Template = new DataTemplate (() => {
								var v = new Label {
									BackgroundColor = Color.Pink,
									TextColor = Color.Black,
									XAlign = TextAlignment.Center,
									YAlign = TextAlignment.Center,
								};
								v.SetBinding(Label.TextProperty, "DoubleX");
								return v;
							}),
						};
						if (oldc != null) {
							oldc.Width = 300;
						}
					},
				},
				new WordPair { 
					English = "feel", 
					Spanish = "sentir" ,
					ButtonText = "Corner view replace", 
					Action = () => {
						if (dg.FrozenColumn != null) {
							dg.FrozenColumn.HeaderView = new DatePicker();
						}
					},
				},
				new WordPair { 
					English = "dine", 
					Spanish = "cenar" ,
					ButtonText = "C[-1] null", 
					Action = () => {
						dg.FrozenColumn = null;
					},
				},

				// TODO rows collecton empty?
				// TODO test add rows when there is less than a screenful of rows
				// TODO delete/replace/insert a row offscreen
				// TODO delete/replace/insert a column offscreen
				// TODO prop chgs on things that are offscreen
				// TODO replace the whole columns collection
				// TODO chg the overall size

			};
		}
	}
}

