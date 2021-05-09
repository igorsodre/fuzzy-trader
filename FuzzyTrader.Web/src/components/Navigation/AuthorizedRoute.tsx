import React from 'react';
import { Redirect, Route } from 'react-router-dom';
import { AppContext, IAppContext } from '../../data/app-context';

class AuthorizedRoute extends Route {
  static contextType = AppContext;
  context!: IAppContext;
  render(): React.ReactNode {
    if (!this.context.state.token) return <Redirect to="/login" />;
    else return super.render();
  }
}

export default AuthorizedRoute;
