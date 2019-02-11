import { post } from '../../helpers/http';

export const PROFILE_REQUESTED = 'profile/PROFILE_REQUESTED';
export const PROFILE_RECEIVED = 'profile/PROFILE_RECEIVED';
export const SEND_VERIFICATION_EMAIL_REQUESTED = 'profile/SEND_VERIFICATION_EMAIL_REQUESTED';
export const SEND_VERIFICATION_EMAIL_RECEIVED = 'profile/SEND_VERIFICATION_EMAIL_RECEIVED';

const initialState = {
    profile: null,
    isLoading: false,
    errorMessages: []
};

export const reducer = (state = initialState, action) => {
    switch (action.type) {
        case PROFILE_REQUESTED:
            return {
                ...state,
                profile: null,
                isLoading: true,
                errorMessages: []
            };

        case PROFILE_RECEIVED:
            return {
                ...state,
                profile: action.success ? action.profile : state.profile,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        case SEND_VERIFICATION_EMAIL_REQUESTED:
            return {
                ...state,
                isLoading: true,
                errorMessages: []
            };

        case SEND_VERIFICATION_EMAIL_RECEIVED:
            return {
                ...state,
                isLoading: false,
                errorMessages: action.success ? [] : action.errorMessages
            };

        default:
            return state;
    }
};


export const fetchProfile = () => {
    return async dispatch => {
        dispatch({
            type: PROFILE_REQUESTED
        });

        var response = await fetch('/api/profile/');
        dispatch({
            type: PROFILE_RECEIVED,
            ...response
        });
    };
};

export const sendVerificationEmail = () => {
    return async dispatch => {
        dispatch({
            type: SEND_VERIFICATION_EMAIL_REQUESTED
        });

        var response = await post('/api/account/sendverificationemail');
        dispatch({
            type: SEND_VERIFICATION_EMAIL_RECEIVED,
            ...response
        });
    };
};

export default {
    fetchProfile,
    sendVerificationEmail
};
