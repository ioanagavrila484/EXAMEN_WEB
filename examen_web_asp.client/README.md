GO
USE WEB_EXAMEN
GO

CREATE TABLE Utilizator(
	Id INT PRIMARY KEY IDENTITY(1,1),
	UserName VARCHAR(100),
)

INSERT INTO Utilizator(UserName) VALUES ('Ioana Gavrila')

CREATE TABLE Ingredient(
	Id INT PRIMARY KEY IDENTITY(1,1),
	NameIngredient VARCHAR(100),
	Unit VARCHAR(100),
	CaloriesPer100g INT,

)

INSERT INTO Ingredient(NameIngredient, Unit,CaloriesPerUnit) VALUES('Rosii', 23,120);
SELECT * FROM INGREDIENTS



CREATE TABLE Recipe(
	Id INT PRIMARY KEY IDENTITY(1,1),
	userId INT FOREIGN KEY REFERENCES Utilizator(Id),
	title VARCHAR(100),
	TotalCalories INT,

);

CREATE TABLE RecipeStep(
	Id INT PRIMARY KEY IDENTITY(1,1),
	recipeId   INT FOREIGN KEY REFERENCES Recipe(Id),
	StepNumber INT,
	DescriptionStep VARCHAR(100),
	IngredientsIds VARCHAR(100)
)

THE USER SHOULD AUTHENTIFICATE  PRIOR TO USINGTHE APPLICATION BY NAME. after auth the user bigins to build a recipe - ordered list of steps. each step has a description and one or more ingredients
multi step flow:
page 1 - title: the user enters the title and click next

page 2 - step builder - the user adds steps to the recipe one at a time, for each step - user writes description and slects one or more ingredients. after adding a step the page refreshes to show all steps added so far and allows the user to add
another step.the user can also remove; continue to do that until he clicks finish

page 3 - review and confirm - user sees full summarry of the recipe - title all step etc also the total calorie count is compited and shown. the user can discard or confirm. also on page 3, the app should analyse the food group distribution. TO HAVE ; the user can delete a recipe
