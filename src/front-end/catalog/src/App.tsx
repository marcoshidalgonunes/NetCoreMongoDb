import { Component}  from 'react';
import { hasAuthParams, withAuth, AuthContextProps } from "react-oidc-context";

import './App.css';

import Router from "./components/Router"

class App extends Component {
  hasTriedSignin: boolean = false;

  render() {
    // const auth = this.props as AuthContextProps;
    // if (!(hasAuthParams() || auth.isAuthenticated || auth.activeNavigator || auth.isLoading || this.hasTriedSignin)) {
    //   auth.signinRedirect();
    // }    

    // if (auth.isLoading) {
    //   return <div>Loading...</div>;
    // }
  
    // if (auth.error) {
    //   return ErrorMessage(auth.error.message);
    // }
  
    // if (auth.isAuthenticated) {
    //   return (
    //     <Router/> 
    //   );
    // }  
    
    // return (
    //   <div className='alert alert-light'>Authenticating...</div>
    // );
    return (
      <Router/> 
    );
  }
}

export default withAuth(App);