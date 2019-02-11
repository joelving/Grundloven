import { post, postJson } from '../../helpers/http';
import { PROFILE_RECEIVED } from './profile';

export const REGISTER_REQUESTED = 'account/REGISTER_REQUESTED';
export const REGISTER_RECEIVED = 'account/REGISTER_RECEIVED';
export const LOGIN_REQUESTED = 'account/LOGIN_REQUESTED';
export const LOGIN_RECEIVED = 'account/LOGIN_RECEIVED';
export const LOGOUT_REQUESTED = 'account/LOGOUT_REQUESTED';
export const LOGOUT_RECEIVED = 'account/LOGOUT_RECEIVED';
export const CONFIRM_EMAIL_REQUESTED = 'account/CONFIRM_EMAIL_REQUESTED';
export const CONFIRM_EMAIL_RECEIVED = 'account/CONFIRM_EMAIL_RECEIVED';
export const FORGET_PASSWORD_REQUESTED = 'account/FORGET_PASSWORD_REQUESTED';
export const FORGET_PASSWORD_RECEIVED = 'account/FORGET_PASSWORD_RECEIVED';
export const RESET_PASSWORD_REQUESTED = 'account/RESET_PASSWORD_REQUESTED';
export const RESET_PASSWORD_RECEIVED = 'account/RESET_PASSWORD_RECEIVED';

const initialState = {
    loggedIn: false,
    isLoading: false,
    errorMessages: []
};

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case REGISTER_REQUESTED:
            return {
                ...state,
                loggedIn: false,
                isLoading: true,
                errorMessages: []
            };

        case REGISTER_RECEIVED:
            return {
                ...state,
                loggedIn: action.success ? true : state.loggedIn,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case LOGIN_REQUESTED:
            return {
                ...state,
                loggedIn: false,
                isLoading: true,
                errorMessages: []
            };

        case LOGIN_RECEIVED:
            return {
                ...state,
                loggedIn: action.success ? true : state.loggedIn,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case LOGOUT_REQUESTED:
            return {
                ...state,
                isLoading: true,
                errorMessages: []
            };

        case LOGOUT_RECEIVED:
            return {
                ...state,
                loggedIn: action.success ? false : state.loggedIn,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case CONFIRM_EMAIL_REQUESTED:
            return {
                ...state,
                isLoading: true,
                errorMessages: []
            };

        case CONFIRM_EMAIL_RECEIVED:
            return {
                ...state,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case FORGET_PASSWORD_REQUESTED:
            return {
                ...state,
                isLoading: true,
                errorMessages: []
            };

        case FORGET_PASSWORD_RECEIVED:
            return {
                ...state,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case RESET_PASSWORD_REQUESTED:
            return {
                ...state,
                isLoading: true,
                errorMessages: []
            };

        case RESET_PASSWORD_RECEIVED:
            return {
                ...state,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        default:
            return state;
    }
};


export const register = (username, password) => {
    return async dispatch => {
        dispatch({
            type: REGISTER_REQUESTED
        });

        var response = await postJson('/api/account/register', { username, password });
        dispatch({
            type: REGISTER_RECEIVED,
            ...response
        });
        dispatch({
            type: PROFILE_RECEIVED,
            ...response
        });
    };
};

export const login = (username, password) => {
    return async dispatch => {
        dispatch({
            type: LOGIN_REQUESTED
        });

        var response = await postJson('/api/account/login', { username, password });
        dispatch({
            type: LOGIN_RECEIVED,
            ...response
        });
        dispatch({
            type: PROFILE_RECEIVED,
            ...response
        });
    };
};

export const logout = () => {
    return async dispatch => {
        dispatch({
            type: LOGOUT_REQUESTED
        });

        var response = await post('/api/account/logout');
        dispatch({
            type: LOGOUT_RECEIVED,
            ...response
        });
    };
};

export const confirmEmail = (code) => {
    return async dispatch => {
        dispatch({
            type: CONFIRM_EMAIL_REQUESTED
        });

        var response = await postJson('/api/account/confirmemail', { code });
        dispatch({
            type: CONFIRM_EMAIL_RECEIVED,
            ...response
        });
    };
};

export const forgotPassword = (email) => {
    return async dispatch => {
        dispatch({
            type: FORGET_PASSWORD_REQUESTED
        });

        var response = await postJson('/api/account/forgotpassword', { email });
        dispatch({
            type: FORGET_PASSWORD_RECEIVED,
            ...response
        });
    };
};

export const resetPassword = (email, password, code) => {
    return async dispatch => {
        dispatch({
            type: RESET_PASSWORD_REQUESTED
        });

        var response = await postJson('/api/account/resetpassword', { email, password, code });
        dispatch({
            type: RESET_PASSWORD_RECEIVED,
            ...response
        });
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
