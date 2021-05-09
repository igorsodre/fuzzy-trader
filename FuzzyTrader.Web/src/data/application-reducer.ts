import { AppMessage } from './models/app-message';
/* eslint-disable @typescript-eslint/no-explicit-any */
// eslint-disable-next-line @typescript-eslint/triple-slash-reference
/// <reference path="../global.d.ts" />
import * as R from 'ramda';

import { AppUser } from './models/user-interface';
import { AuthService } from './../hooks/auth-service';
import { RefreshTokenResponse } from './contracts/responses/account';

export interface ApplicationState {
  token: string;
  currentUser: Nullable<AppUser>;
  isApplicationLoading: boolean;
  appMessages: AppMessage[];
}

export interface ApplicationActions {
  logout: () => Promise<void>;
  setAccessToken: (token: string) => void;
  setCurrentUser: (user: AppUser) => void;
  onRouteChange: () => void;
  setIsApplicationLoading: (isLoading: boolean) => void;
  startUp: () => Promise<void>;
}

export enum ApplicationActionType {
  LOGOUT,
  SET_ACCESS_TOKEN,
  SET_CURRENT_USER,
  ON_ROUTE_CHANGE,
  START_UP,
}

type TReducer<T = any> = (state: ApplicationState, action: ApplicationAction<T>) => ApplicationState;

export interface ApplicationAction<T = any> {
  type: ApplicationActionType;
  payload: Nullable<T>;
}

export const APP_INITIAL_STATE: ApplicationState = {
  currentUser: null,
  token: '',
  isApplicationLoading: true,
  appMessages: [],
};

const logoutReducer: TReducer<string> = (state, _action) => {
  const newState = R.clone(state);
  newState.token = '';
  newState.currentUser = null;
  return state;
};

const setAccessTokenReducer: TReducer<string> = (state, action) => {
  const newState = R.clone(state);
  newState.token = action.payload || '';
  return state;
};

const setCurrentUserReducer: TReducer<AppUser> = (state, action) => {
  const newState = R.clone(state);
  newState.currentUser = action.payload;
  return state;
};

const onRouteChangeReducer: TReducer<string> = (state, _action) => {
  const newState = R.clone(state);
  newState.appMessages = [];
  return state;
};

const startUpReducer: TReducer<RefreshTokenResponse> = (state, action) => {
  const newState = R.clone(state);
  newState.token = action.payload?.accessToken || '';
  newState.currentUser = action.payload?.user;
  newState.isApplicationLoading = false;
  return state;
};

const logout = (dispatch: React.Dispatch<ApplicationAction>, authService: AuthService) => {
  return async () => {
    await authService.logout();
    dispatch({ type: ApplicationActionType.LOGOUT, payload: null });
  };
};

const startUp = (dispatch: React.Dispatch<ApplicationAction<RefreshTokenResponse>>, authService: AuthService) => {
  return async () => {
    const result = await authService.refreshToken();
    dispatch({ type: ApplicationActionType.LOGOUT, payload: result });
  };
};

const setAccessToken = (dispatch: React.Dispatch<ApplicationAction<string>>) => {
  return (accessToken: string) => {
    dispatch({ type: ApplicationActionType.SET_ACCESS_TOKEN, payload: accessToken });
  };
};

const setCurrentUser = (dispatch: React.Dispatch<ApplicationAction<AppUser>>) => {
  return (user: AppUser) => {
    dispatch({ type: ApplicationActionType.SET_ACCESS_TOKEN, payload: user });
  };
};

const setIsApplicationLoading = (dispatch: React.Dispatch<ApplicationAction<boolean>>) => {
  return (isApplicationLoading: boolean) => {
    dispatch({ type: ApplicationActionType.SET_ACCESS_TOKEN, payload: isApplicationLoading });
  };
};

const onRouteChange = (dispatch: React.Dispatch<ApplicationAction<null>>) => {
  return () => {
    dispatch({ type: ApplicationActionType.SET_ACCESS_TOKEN, payload: null });
  };
};

export const actionFactory = (
  dispatcher: React.Dispatch<ApplicationAction<any>>,
  authService: AuthService,
): ApplicationActions => {
  return {
    startUp: startUp(dispatcher, authService),
    logout: logout(dispatcher, authService),
    setAccessToken: setAccessToken(dispatcher),
    setCurrentUser: setCurrentUser(dispatcher),
    setIsApplicationLoading: setIsApplicationLoading(dispatcher),
    onRouteChange: onRouteChange(dispatcher),
  };
};

export const globalReducer: TReducer = (state, action) => {
  switch (action.type) {
    case ApplicationActionType.START_UP:
      return startUpReducer(state, action);
    case ApplicationActionType.LOGOUT:
      return logoutReducer(state, action);
    case ApplicationActionType.SET_ACCESS_TOKEN:
      return setAccessTokenReducer(state, action);
    case ApplicationActionType.SET_CURRENT_USER:
      return setCurrentUserReducer(state, action);
    case ApplicationActionType.ON_ROUTE_CHANGE:
      return onRouteChangeReducer(state, action);
    default:
      return state;
  }
};
