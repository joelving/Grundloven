import { post } from '../../helpers/http';
import auth from '../../helpers/auth';
import { push } from 'connected-react-router';
import { defaultRequestState, setInitialRequestState, setSucceededRequestState, setFailedRequestState } from '../../helpers/api-request-reducer-helpers';

export const REGISTER_REQUESTED = 'account/REGISTER_REQUESTED';
export const REGISTER_SUCCEEDED = 'account/REGISTER_SUCCEEDED';
export const REGISTER_FAILED = 'account/REGISTER_FAILED';

export const CONFIRM_EMAIL_REQUESTED = 'account/CONFIRM_EMAIL_REQUESTED';
export const CONFIRM_EMAIL_SUCCEEDED = 'account/CONFIRM_EMAIL_SUCCEEDED';
export const CONFIRM_EMAIL_FAILED = 'account/CONFIRM_EMAIL_FAILED';

export const LOGIN_REQUESTED = 'account/LOGIN_REQUESTED';
export const LOGIN_SUCCEEDED = 'account/LOGIN_SUCCEEDED';
export const LOGIN_FAILED = 'account/LOGIN_FAILED';

export const LOGOUT_REQUESTED = 'account/LOGOUT_REQUESTED';
export const LOGOUT_SUCCEEDED = 'account/LOGOUT_SUCCEEDED';
export const LOGOUT_FAILED = 'account/LOGOUT_FAILED';

export const FORGET_PASSWORD_REQUESTED = 'account/FORGET_PASSWORD_REQUESTED';
export const FORGET_PASSWORD_SUCCEEDED = 'account/FORGET_PASSWORD_SUCCEEDED';
export const FORGET_PASSWORD_FAILED = 'account/FORGET_PASSWORD_FAILED';

export const RESET_PASSWORD_REQUESTED = 'account/RESET_PASSWORD_REQUESTED';
export const RESET_PASSWORD_SUCCEEDED = 'account/RESET_PASSWORD_SUCCEEDED';
export const RESET_PASSWORD_FAILED = 'account/RESET_PASSWORD_FAILED';

const initialState = {
    loggedIn: auth.getToken() !== null,
    register: Object.assign({}, defaultRequestState),
    login: Object.assign({}, defaultRequestState),
    logout: Object.assign({}, defaultRequestState),
    confirmEmail: Object.assign({}, defaultRequestState),
    forgotPassword: Object.assign({}, defaultRequestState),
    resetPassword: Object.assign({}, defaultRequestState)
};

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case REGISTER_REQUESTED:
            return setInitialRequestState('register');
        case REGISTER_SUCCEEDED:
            return setSucceededRequestState('register');
        case REGISTER_FAILED:
            return setFailedRequestState('register');

        case CONFIRM_EMAIL_REQUESTED:
            return setInitialRequestState('confirmEmail');
        case CONFIRM_EMAIL_SUCCEEDED:
            return setSucceededRequestState('confirmEmail');
        case CONFIRM_EMAIL_FAILED:
            return setFailedRequestState('confirmEmail');

        case LOGIN_REQUESTED:
            return setInitialRequestState('login');
        case LOGIN_SUCCEEDED:
            auth.setToken(action);
            return setSucceededRequestState('login');
        case LOGIN_FAILED:
            return setFailedRequestState('login');

        case LOGOUT_REQUESTED:
            return setInitialRequestState('login');
        case LOGOUT_SUCCEEDED:
            auth.removeToken();
            return setSucceededRequestState('login');
        case LOGOUT_FAILED:
            return setFailedRequestState('login');

        case FORGET_PASSWORD_REQUESTED:
            return setInitialRequestState('forgotPassword');
        case FORGET_PASSWORD_SUCCEEDED:
            return setSucceededRequestState('forgotPassword');
        case FORGET_PASSWORD_FAILED:
            return setFailedRequestState('forgotPassword');

        case RESET_PASSWORD_REQUESTED:
            return setInitialRequestState('resetPassword');
        case RESET_PASSWORD_SUCCEEDED:
            return setSucceededRequestState('resetPassword');
        case RESET_PASSWORD_FAILED:
            return setFailedRequestState('resetPassword');

        default:
            return state;
    }
};


export const register = (email, password) => {
    return async dispatch => {
        dispatch({ type: REGISTER_REQUESTED });

        try {
            let response = await post('/api/account/register', { email, password });
            dispatch({ type: REGISTER_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REGISTER_FAILED, problemDetails });
        }
    };
};

export const login = (username, password) => {
    return async dispatch => {
        dispatch({ type: LOGIN_REQUESTED });

        try {
            let response = await post('/api/account/login', { grant_type: "password", username, password }, false);
            dispatch({ type: LOGIN_SUCCEEDED, ...response });
            dispatch(push('/'))
        }
        catch (problemDetails) {
            dispatch({ type: LOGIN_FAILED, problemDetails });
        }
    };
};

export const logout = () => {
    return async dispatch => {
        dispatch({ type: LOGOUT_REQUESTED });

        try {
            let response = await post('/api/account/logout');
            dispatch({ type: LOGOUT_SUCCEEDED, ...response });
            dispatch(push('/'))
        }
        catch (problemDetails) {
            dispatch({ type: LOGOUT_FAILED, problemDetails });
        }
    };
};

export const confirmEmail = (code) => {
    return async dispatch => {
        dispatch({ type: CONFIRM_EMAIL_REQUESTED });

        try {
            var response = await post('/api/account/confirm-email', { code });
            dispatch({ type: CONFIRM_EMAIL_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: CONFIRM_EMAIL_FAILED, problemDetails });
        }
    };
};

export const forgotPassword = (email) => {
    return async dispatch => {
        dispatch({ type: FORGET_PASSWORD_REQUESTED });

        try {
            var response = await post('/api/account/forgot-password', { email });
            dispatch({ type: FORGET_PASSWORD_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: FORGET_PASSWORD_FAILED, problemDetails });
        }
    };
};

export const resetPassword = (email, password, code) => {
    return async dispatch => {
        dispatch({ type: RESET_PASSWORD_REQUESTED });

        try {
            var response = await post('/api/account/reset-password', { email, password, code });
            dispatch({ type: RESET_PASSWORD_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: RESET_PASSWORD_FAILED, problemDetails });
        }
    };
};

export default {
    register,
    confirmEmail,
    login,
    logout,
    forgotPassword,
    resetPassword
};
