import { post, postJson, postForm } from '../../helpers/http';
import auth from '../../helpers/auth';

export const REQUEST_FAILED = 'account/REQUEST_FAILED';
export const REGISTER_REQUESTED = 'account/REGISTER_REQUESTED';
export const REGISTER_SUCCEEDED = 'account/REGISTER_SUCCEEDED';
export const LOGIN_REQUESTED = 'account/LOGIN_REQUESTED';
export const LOGIN_SUCCEEDED = 'account/LOGIN_SUCCEEDED';
export const LOGOUT_REQUESTED = 'account/LOGOUT_REQUESTED';
export const LOGOUT_SUCCEEDED = 'account/LOGOUT_SUCCEEDED';
export const CONFIRM_EMAIL_REQUESTED = 'account/CONFIRM_EMAIL_REQUESTED';
export const CONFIRM_EMAIL_SUCCEEDED = 'account/CONFIRM_EMAIL_SUCCEEDED';
export const FORGET_PASSWORD_REQUESTED = 'account/FORGET_PASSWORD_REQUESTED';
export const FORGET_PASSWORD_SUCCEEDED = 'account/FORGET_PASSWORD_SUCCEEDED';
export const RESET_PASSWORD_REQUESTED = 'account/RESET_PASSWORD_REQUESTED';
export const RESET_PASSWORD_SUCCEEDED = 'account/RESET_PASSWORD_SUCCEEDED';

const initialState = {
    loggedIn: false,
    isLoading: false,
    problemDetails: null
};

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case REQUEST_FAILED:
            return {
                ...state,
                isLoading: false,
                problemDetails: action.problemDetails
            };

        case REGISTER_REQUESTED:
            auth.removeToken();
            return {
                ...state,
                loggedIn: false,
                isLoading: true,
                problemDetails: null
            };
        case REGISTER_SUCCEEDED:
            return {
                ...state,
                isLoading: false
            };

        case LOGIN_REQUESTED:
            auth.removeToken();
            return {
                ...state,
                loggedIn: false,
                isLoading: true,
                problemDetails: null
            };
        case LOGIN_SUCCEEDED:
            auth.setToken(action);
            return {
                ...state,
                loggedIn: true,
                isLoading: false
            };

        case LOGOUT_REQUESTED:
            return {
                ...state,
                isLoading: true,
                problemDetails: null
            };
        case LOGOUT_SUCCEEDED:
            auth.removeToken();
            return {
                ...state,
                loggedIn: false,
                isLoading: false
            };

        case CONFIRM_EMAIL_REQUESTED:
            return {
                ...state,
                isLoading: true,
                problemDetails: null
            };
        case CONFIRM_EMAIL_SUCCEEDED:
            return {
                ...state,
                isLoading: false
            };

        case FORGET_PASSWORD_REQUESTED:
            return {
                ...state,
                isLoading: true,
                problemDetails: null
            };
        case FORGET_PASSWORD_SUCCEEDED:
            return {
                ...state,
                isLoading: false
            };

        case RESET_PASSWORD_REQUESTED:
            return {
                ...state,
                isLoading: true,
                problemDetails: null
            };

        case RESET_PASSWORD_SUCCEEDED:
            return {
                ...state,
                isLoading: false
            };

        default:
            return state;
    }
};


export const register = (email, password) => {
    return async dispatch => {
        dispatch({
            type: REGISTER_REQUESTED
        });

        try {
            let response = await post('/api/account/register', { email, password });
            await dispatch({
                type: REGISTER_SUCCEEDED,
                ...response
            });
            login(email, password);
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export const login = (username, password) => {
    return async dispatch => {
        dispatch({
            type: LOGIN_REQUESTED
        });

        try {
            let response = await post('/api/account/login', { grant_type: "password", username, password }, false);
            dispatch({ type: LOGIN_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export const logout = () => {
    return async dispatch => {
        dispatch({
            type: LOGOUT_REQUESTED
        });

        try {
            let response = await post('/api/account/logout');
            dispatch({ type: LOGOUT_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export const confirmEmail = (code) => {
    return async dispatch => {
        dispatch({
            type: CONFIRM_EMAIL_REQUESTED
        });

        try {
            var response = await post('/api/account/confirm-email', { code });
            dispatch({ type: CONFIRM_EMAIL_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export const forgotPassword = (email) => {
    return async dispatch => {
        dispatch({
            type: FORGET_PASSWORD_REQUESTED
        });

        try {
            var response = await post('/api/account/forgot-password', { email });
            dispatch({ type: FORGET_PASSWORD_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export const resetPassword = (email, password, code) => {
    return async dispatch => {
        dispatch({
            type: RESET_PASSWORD_REQUESTED
        });

        try {
            var response = await post('/api/account/reset-password', { email, password, code });
            dispatch({ type: RESET_PASSWORD_SUCCEEDED, ...response });
        }
        catch (problemDetails) {
            dispatch({ type: REQUEST_FAILED, problemDetails });
        }
    };
};

export default {
    register,
    login,
    logout,
    confirmEmail,
    forgotPassword,
    resetPassword
};
