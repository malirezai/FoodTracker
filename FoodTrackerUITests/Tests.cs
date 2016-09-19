using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace FoodTrackerUITests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;

		const int RATING = 3;
		const string NAME = "Mixed Salad";

		Query FoodCell;

		public Tests(Platform platform)
		{
			this.platform = platform;
			InitializeQueries();
		}

		void InitializeQueries()
		{
			FoodCell = x => x.Marked(NAME);

		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = AppInitializer.StartApp(platform);
		}


		[Test]
		public void EditFoodTest()
		{
			//app.Repl();
			app.Tap(x => x.Marked("Caprese Salad"));
			app.Screenshot("Tapped on Caprese Salad");
			app.Tap(x => x.Class("UITextFieldLabel").Marked("Caprese Salad"));
			app.Screenshot("Editing Text");
			app.ClearText(x => x.Class("UITextField").Text("Caprese Salad"));
			app.EnterText(x => x.Class("UITextField"), "Caprese Mixed Salad");
			app.PressEnter();
			app.Tap(x => x.Id("filledStar").Index(RATING - 1));
			app.Screenshot("Tapped on view with class: UIImageView");
			app.Tap(x => x.Text("Save"));
			app.Screenshot("Changed Name and Rating then Press Save");

			//get rating showed
			var rating = app.Query(x => x.Class("UITableViewCellContentView")
			                       .Descendant()
			                       .Marked(NAME)
			                       .Sibling()
			                       .Class("FoodTracker.RatingControl")
			                       .Descendant()
			                       .Id("filledStar"))
			                	   .Count();

			app.Screenshot($"Rating count should be: {rating}");

			Assert.IsTrue(rating == RATING);

		}


	}
}

