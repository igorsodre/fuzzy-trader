import React, { createContext, useEffect, useReducer } from 'react';
import {
  actionFactory,
  ApplicationActions,
  ApplicationState,
  APP_INITIAL_STATE,
  globalReducer,
} from './application-reducer';
import { useAuth } from '../hooks/auth-service';

export interface IAppContext {
  state: ApplicationState;
  actions: ApplicationActions;
}

const AppContext = createContext<IAppContext | Record<string, unknown>>({ state: APP_INITIAL_STATE, actions: {} });

const AppContextProvider: React.FC = (props) => {
  const auth = useAuth();
  const [globalState, dispatch] = useReducer(globalReducer, APP_INITIAL_STATE);
  const globalActions = actionFactory(dispatch, auth);

  useEffect(() => {
    globalActions.startUp().catch((err) => {
      console.log(err);
    });
  }, []);

  return (
    <AppContext.Provider
      value={
        {
          state: globalState,
          actions: globalActions,
        } as IAppContext
      }
    >
      {props.children}
    </AppContext.Provider>
  );
};

export { AppContext, AppContextProvider };
