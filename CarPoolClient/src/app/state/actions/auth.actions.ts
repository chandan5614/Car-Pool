import { createAction, props } from '@ngrx/store';
import { LoginModel } from '../../models/login.model';
import { RegisterModel } from '../../models/register.model';

// Registration Actions
export const register = createAction(
  '[Auth] Register',
  props<{ user: RegisterModel }>()
);

export const registerSuccess = createAction(
  '[Auth] Register Success',
  props<{ user: any }>()  // Adjust the type as needed
);

export const registerFailure = createAction(
  '[Auth] Register Failure',
  props<{ error: any }>()  // Adjust the type as needed
);

// Login Actions
export const login = createAction(
  '[Auth] Login',
  props<{ user: LoginModel }>()
);

export const loginSuccess = createAction(
  '[Auth] Login Success',
  props<{ user: any }>()  // Adjust the type as needed
);

export const loginFailure = createAction(
  '[Auth] Login Failure',
  props<{ error: any }>()  // Adjust the type as needed
);
