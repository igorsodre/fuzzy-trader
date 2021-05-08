import React, { createContext, useCallback, useEffect, useState } from 'react';
import { useAuth } from '../hooks/auth-service';
import { AppUser } from './models/user-interface';

export interface IAppContext {
  logout: () => void;
  setAccessToken: (token: string) => void;
  token: Nullable<string>;
  currentUser: Nullable<AppUser>;
  setCurrentUser: (user: AppUser) => void;
  wholeAppIsLoading: boolean;
  onRouteChange: (pathName?: string) => void;
}

const AppContext = createContext<IAppContext | Record<string, unknown>>({});

const AppContextProvider: React.FC = (props) => {
  const [token, setAccessToken] = useState<Nullable<string>>(null);
  const [currentUser, setCurrentUser] = useState<Nullable<AppUser>>();
  const { refreshToken, logout: doLogout } = useAuth();
  const [wholeAppIsLoading, setWholeAppIsLoading] = useState(true);

  const logout = useCallback(async () => {
    try {
      await doLogout();
      setAccessToken('');
    } catch (err) {
      console.log(err);
    }
  }, [doLogout]);

  const onRouteChange = (pathName?: string) => {
    console.log('Changed route: ' + pathName);
  };

  useEffect(() => {
    refreshToken()
      .then((res) => {
        setWholeAppIsLoading(false);
        if (res.accessToken) {
          setAccessToken(res.accessToken);
          setCurrentUser(res.user);
        }
      })
      .catch((err) => {
        console.log(err);
      });
  }, [refreshToken]);

  return (
    <AppContext.Provider
      value={
        { token, logout, setAccessToken, currentUser, setCurrentUser, wholeAppIsLoading, onRouteChange } as IAppContext
      }
    >
      {props.children}
    </AppContext.Provider>
  );
};

export { AppContext, AppContextProvider };
