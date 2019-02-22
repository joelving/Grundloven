import { createStore, applyMiddleware, compose, combineReducers } from 'redux';
import { connectRouter, routerMiddleware } from 'connected-react-router';
import thunk from 'redux-thunk';
import createHistory from 'history/createBrowserHistory';
import callAPIMiddleware from './callAPIMiddleware';

import { reducer as account} from './reducers/account';
import { reducer as profile} from './reducers/profile';
const rootReducer = combineReducers({
    account,
    profile
});

export const history = createHistory();

const initialState = {};
const enhancers = [];
const middleware = [thunk, routerMiddleware(history), callAPIMiddleware];

if (process.env.NODE_ENV === 'development') {
    const devToolsExtension = window.__REDUX_DEVTOOLS_EXTENSION__;

  if (typeof devToolsExtension === 'function') {
      enhancers.push(devToolsExtension());
  }
}

const composedEnhancers = compose(
    applyMiddleware(...middleware),
    ...enhancers
);

export default createStore(
    connectRouter(history)(rootReducer),
    initialState,
    composedEnhancers
);
