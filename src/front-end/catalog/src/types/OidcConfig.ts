import { WebStorageStateStore } from "oidc-client-ts";
import { AuthProviderProps } from "react-oidc-context";

const OidcConfig: AuthProviderProps = {
    authority: process.env.REACT_APP_AUTHORITY,
    client_id: process.env.REACT_APP_CLIENT_ID,
    redirect_uri: process.env.REACT_APP_REDIRECT_URI,
    userStore: new WebStorageStateStore({ store: window.localStorage }),
    onSigninCallback: (args: any) =>
      window.history.replaceState(
        {},
        document.title,
        window.location.pathname
      )
  };

export default OidcConfig;