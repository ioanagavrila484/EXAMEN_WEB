import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { forkJoin } from 'rxjs';

interface LoginResult {
  id: number;
  userName: string;
}

interface Ingredient {
  id: number;
  nameIngredient: string;
  unit: string;
  caloriesPer100g: number;
}

interface LocalStep {
  stepNumber: number;
  descriptionStep: string;
  ingredientIds: number[];
}

interface FoodGroup {
  label: string;
  count: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: false,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'examen_web_asp.client';

  // ===== LOGIN =====
  public username = '';
  public loggedInUser: LoginResult | null = null;
  public errorMessage = '';

  // ===== PAGES: 0=login, 1=title, 2=steps, 3=review, 4=saved =====
  public currentPage = 0;

  // ===== PAGE 1 =====
  public recipeTitle = '';

  // ===== PAGE 2 =====
  public ingredients: Ingredient[] = [];
  public stepDescription = '';
  public selectedIngredientIds: number[] = [];
  public steps: LocalStep[] = [];

  // ===== PAGE 3 =====
  public totalCalories = 0;
  public foodGroups: FoodGroup[] = [];

  // ===== PAGE 4 =====
  public savedRecipeId = 0;

  constructor(private http: HttpClient) {}

  // ===== LOGIN =====
  login() {
    this.errorMessage = '';
    this.http.post<LoginResult>(`/api/auth/login?userName=${encodeURIComponent(this.username)}`, null).subscribe(
      (result: LoginResult) => {
        this.loggedInUser = result;
        this.currentPage = 1;
      },
      (error: any) => {
        this.errorMessage = 'Utilizator inexistent';
        console.error(error);
      }
    );
  }

  // ===== PAGE 1 -> PAGE 2 =====
  goToStepBuilder() {
    if (!this.recipeTitle.trim()) return;
    this.steps = [];
    this.http.get<Ingredient[]>('/api/ingredient/all').subscribe(
      (result: Ingredient[]) => {
        this.ingredients = result;
        this.currentPage = 2;
      },
      (error: any) => { console.error(error); }
    );
  }

  // ===== PAGE 2 HELPERS =====
  isSelected(id: number): boolean {
    return this.selectedIngredientIds.includes(id);
  }

  toggleIngredient(id: number) {
    const idx = this.selectedIngredientIds.indexOf(id);
    if (idx >= 0) {
      this.selectedIngredientIds.splice(idx, 1);
    } else {
      this.selectedIngredientIds.push(id);
    }
  }

  addStep() {
    if (!this.stepDescription.trim() || this.selectedIngredientIds.length === 0) return;
    this.steps.push({
      stepNumber: this.steps.length + 1,
      descriptionStep: this.stepDescription,
      ingredientIds: [...this.selectedIngredientIds]
    });
    this.stepDescription = '';
    this.selectedIngredientIds = [];
  }

  removeStep(index: number) {
    this.steps.splice(index, 1);
    this.steps.forEach((s, i) => s.stepNumber = i + 1);
  }

  getIngredientNames(ids: number[]): string {
    return ids
      .map(id => this.ingredients.find(i => i.id === id)?.nameIngredient ?? '')
      .join(', ');
  }

  // ===== PAGE 2 -> PAGE 3 =====
  goToReview() {
    if (this.steps.length === 0) return;

    const usedIds = new Set(this.steps.flatMap(s => s.ingredientIds));
    const usedIngredients = Array.from(usedIds)
      .map(id => this.ingredients.find(i => i.id === id))
      .filter((i): i is Ingredient => i !== undefined);

    this.totalCalories = usedIngredients.reduce((sum, i) => sum + i.caloriesPer100g, 0);

    this.foodGroups = [
      { label: 'Calorii scazute (< 100 cal/100g)',    count: usedIngredients.filter(i => i.caloriesPer100g < 100).length },
      { label: 'Calorii medii (100-299 cal/100g)',     count: usedIngredients.filter(i => i.caloriesPer100g >= 100 && i.caloriesPer100g < 300).length },
      { label: 'Calorii ridicate (>= 300 cal/100g)',   count: usedIngredients.filter(i => i.caloriesPer100g >= 300).length }
    ];

    this.currentPage = 3;
  }

  // ===== PAGE 3 ACTIONS =====
  discardRecipe() {
    this.recipeTitle = '';
    this.steps = [];
    this.currentPage = 1;
  }

  confirmRecipe() {
    const recipe = {
      userId: this.loggedInUser!.id,
      title: this.recipeTitle,
      totalCalories: this.totalCalories
    };

    this.http.post<{ id: number }>('/api/recipe/create', recipe).subscribe(
      (res: { id: number }) => {
        const recipeId = res.id;
        this.savedRecipeId = recipeId;

        const stepRequests = this.steps.map(s =>
          this.http.post('/api/recipestep/add', {
            recipeId,
            stepNumber: s.stepNumber,
            descriptionStep: s.descriptionStep,
            ingredientsIds: s.ingredientIds.join(',')
          })
        );

        forkJoin(stepRequests).subscribe(
          () => { this.currentPage = 4; },
          (error: any) => { console.error(error); }
        );
      },
      (error: any) => { console.error(error); }
    );
  }

  // ===== PAGE 4 ACTIONS =====
  deleteRecipe() {
    this.http.delete(`/api/recipe/${this.savedRecipeId}`).subscribe(
      () => {
        this.savedRecipeId = 0;
        this.recipeTitle = '';
        this.steps = [];
        this.currentPage = 1;
      },
      (error: any) => { console.error(error); }
    );
  }

  newRecipe() {
    this.savedRecipeId = 0;
    this.recipeTitle = '';
    this.steps = [];
    this.currentPage = 1;
  }
}
