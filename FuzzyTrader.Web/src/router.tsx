import React, { useContext, useEffect } from 'react';
import { BrowserRouter as Router, Redirect, Route, Switch, useLocation } from 'react-router-dom';
import AuthorizedRoute from './components/Navigation/AuthorizedRoute';
import NavBar from './components/Navigation/Navbar';
import MessagesContainer from './components/UiElements/MessagesContainer';
import { AppContext, IAppContext } from './data/app-context';
import ForgotPassword from './pages/ForgotPassword';
import Home from './pages/Home';
import Login from './pages/Login';
import Profile from './pages/Profile';
import ResetPassword from './pages/ResetPassword';
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
    ctx.actions.onRouteChange();
  }, [location]);

  return (
    <div className="App">
      <NavBar />
      <MessagesContainer />
      <Switch>
        <Route path="/" component={Startup} exact />
        <Route path="/signup" component={Signup} exact />
        <Route path="/login" component={Login} exact />
        <Route path="/forgot_password" component={ForgotPassword} exact />
        <Route path="/reset_password" component={ResetPassword} exact />
        <AuthorizedRoute path="/home" component={Home} exact />
        <AuthorizedRoute path="/profile" component={Profile} exact />
        <Redirect to="/" />
      </Switch>
    </div>
  );
};

export default AppRouter;
