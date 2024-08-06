import { Component}  from 'react';

import { hasAuthParams, withAuth, AuthContextProps } from "react-oidc-context";
import { AuthProps } from './types/AuthProps';
import AuthUser from './types/AuthUser';
import Router from "./components/Router"

import './App.css';

type AuthState = { 
  hasTriedSignin: boolean;
}

class App extends Component {
  auth: AuthContextProps = (this.props as AuthProps).auth;
  state: AuthState = { hasTriedSignin: false };

  componentDidMount() {
    const user = AuthUser.getUser();

    if (!user) {
      void this.auth.signinRedirect();
      return;
    }

    this.setState({hasTriedSignin: true}, () => { console.log(`Signed: ${this.state.hasTriedSignin}`) });
  }

  render() {
    if (this.auth.error) {
      return <div className='alert alert-danger mt-3'>An error happened with Catalog Service: <strong>{this.auth.error.message}</strong></div>
    }
  
    if (this.state.hasTriedSignin) {
      return (
        <Router/> 
      );
    }  

    return (
      <div className='alert alert-light'>Authenticating...</div>
    );
  }
}

export default withAuth(App);