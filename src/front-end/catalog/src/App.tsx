import { Component}  from 'react';

import { User, UserManager, WebStorageStateStore } from 'oidc-client-ts'
import { hasAuthParams, withAuth, AuthContextProps } from "react-oidc-context";
import AuthService from './services/AuthService';

import './App.css';

import Router from "./components/Router"

class App extends Component {
  hasTriedSignin: boolean = false;
  authService: AuthService = new AuthService();

  render() {
    const auth = this.props as AuthContextProps;
    if (!(hasAuthParams() || auth.isAuthenticated || auth.activeNavigator || auth.isLoading || this.hasTriedSignin)) {
      this.authService.signInRedirect();
      this.hasTriedSignin = true;
    }    

    if (auth.isLoading) {
      return <div>Loading...</div>;
    }
  
    if (auth.error) {
      //return ErrorMessage(auth.error.message);
    }
  
    if (auth.isAuthenticated) {
      return (
        <Router/> 
      );
    }  
    
    console.log(`HasAuthParams: ${hasAuthParams()}`)
    console.log(`Isloading: ${auth.isLoading}`)
    console.log(`IsAuthenticated: ${auth.isAuthenticated}`)
    console.log(`HasTriedSignin: ${this.hasTriedSignin}`)
    console.log(JSON.stringify(this.props))

    return (
      <div className='alert alert-light'>Authenticating...</div>
    );

  }
}

export default withAuth(App);