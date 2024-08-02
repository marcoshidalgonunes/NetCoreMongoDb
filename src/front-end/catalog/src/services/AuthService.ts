import { User, UserManager } from 'oidc-client-ts'
import OidcConfig from "../types/OidcConfig";

export default class AuthService {
  userManager: UserManager

  constructor() {
    const settings = {
      authority: process.env.REACT_APP_AUTHORITY as string,
      client_id: process.env.REACT_APP_CLIENT_ID as string,
      redirect_uri:  process.env.REACT_APP_REDIRECT_URI as string,
      silent_redirect_uri: `${window.location.origin}/silent-refresh`,
      post_logout_redirect_uri: `${window.location.origin}`,
      response_type: 'code',
      loadUserInfo: true,
    }
    this.userManager = new UserManager(settings)
  }

  public signInRedirect() {
    return this.userManager.signinRedirect()
  }

  public signInCallback() {
    return this.userManager.signinCallback()
  }

  public renewToken(): Promise<void> {
    return this.userManager.signinSilentCallback()
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect()
  }

  public getUser(): Promise<User | null> {
    return this.userManager.getUser()
  }
}