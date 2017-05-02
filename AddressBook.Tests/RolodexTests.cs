using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.Tests
{
    [TestFixture]
    public class RolodexTests
    {
        private IGetInputFromUsers _input;
        private IHandleContacts _contacts;
        private IHandleRecipes _recipes;
        private Rolodex _rolodex;

        [SetUp]
        public void BeforeEachTest()
        {
            //Mock Objects//
            _input = Substitute.For<IGetInputFromUsers>();
            _contacts = Substitute.For<IHandleContacts>();
            _recipes = Substitute.For<IHandleRecipes>();
            _rolodex = new Rolodex(_contacts, _recipes, _input);

        }
        [Test]
        public void ExitJustDoesNothing()
        {
            //Arrange
             _input.GetNumber().Returns(0);

            //Act
            _rolodex.DoStuff();

            //Assert
            _input.Received().GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceiveWithAnyArgs().CreateCompany(null, null);
        }
        [Test]
        public void AddPersonAddsAPersonJustLikeYouWouldExpectItTo()
        {
            //Arrange
            
            _input.GetNumber().Returns(1,0);
            _input.GetNonEmptyString().Returns("Bob", "Marley", "567-678-6878");           

            //Act
            _rolodex.DoStuff();

            //Assert ( make sure the method calls with these reports) //
            _input.Received(2).GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceiveWithAnyArgs().CreateCompany(null, null);
            _contacts.Received().CreatePerson("Bob", "Marley", "567-678-6878");
        }
        [Test]
        public void AddRecipeAddsARecipeJustLikeYouWouldExpectItTo()
        {
            //Arrange

            _input.GetNumber().Returns(6,1,0);
            _input.GetNonEmptyString().Returns("Chicken Alfredo");

            //Act
            _rolodex.DoStuff();

            //Assert ( make sure the method calls with these reports) //
            _input.Received(3).GetNumber();
            _contacts.DidNotReceive().GetAllContacts();
            _recipes.DidNotReceive().GetAllRecipes();
            _contacts.DidNotReceiveWithAnyArgs().CreateCompany(null, null);
            _contacts.DidNotReceiveWithAnyArgs().CreatePerson(null, null, null);
            _recipes.Received().Create("Chicken Alfredo", RecipeType.Entreés);
        }
    }
}
