import React, { useContext, useEffect } from 'react';
import { BrowserRouter as Router, Redirect, Route, Switch, useLocation } from 'react-router-dom';
import AuthorizedRoute from './components/Navigation/AuthorizedRoute';
import NavBar from './components/Navigation/Navbar';
import { AppContext, IAppContext } from './data/app-context';
import Home from './pages/Home';
import Login from './pages/Login';
import Profile from './pages/Profile';
import Signup from './pages/Signup';
import Startup from './pages/Startup';

const AppRouter: React.FC = (props) => {
  return (
    <Router>
      <RouteList />
    </Router>
  );
};

const RouteList: React.FC = () => {
  const location = useLocation();
  const ctx = useContext(AppContext) as IAppContext;
  useEffect(() => {
    ctx.onRouteChange(location.pathname);
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [location]);

  return (
    <div className="App">
      <NavBar />
      <Switch>
        <Route path="/" component={Startup} exact />
        <Route path="/signup" component={Signup} exact />
        <Route path="/login" component={Login} exact />
        <AuthorizedRoute path="/home" component={Home} exact />
        <AuthorizedRoute path="/profile" component={Profile} exact />
        <Redirect to="/" />
      </Switch>
    </div>
  );
};

export default AppRouter;
