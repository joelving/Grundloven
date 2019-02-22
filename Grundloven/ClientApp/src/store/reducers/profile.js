import { post } from '../../helpers/http';
import createReducer from '../createReducer';
import { defaultRequestState, setInitialRequestState, setSucceededRequestState, setFailedRequestState } from '../../helpers/api-request-reducer-helpers';

const namespace = 'profile';
const endpoint = '/api/profile/';

const stores = {
    FETCH_PROFILE: {
        initialState: Object.assign({}, defaultRequestState),
        key: 'fetchProfile',
        reducers: {
            request: (state) => setInitialRequestState(state, 'fetchProfile'),
            success: (state, action) => { let newState = setSucceededRequestState(state, 'fetchProfile'); newState.profile = action.profile; return newState; },
            failed: (state, action) => setFailedRequestState(state, 'fetchProfile', action)
        }
    },
    INITIATE_CHANGE_EMAIL: {
        initialState: Object.assign({}, defaultRequestState),
        key: 'initiateEmailChange',
        reducers: {
            request: (state) => setInitialRequestState(state, 'initiateEmailChange'),
            success: (state) => setSucceededRequestState(state, 'initiateEmailChange'),
            failed: (state, action) => setFailedRequestState(state, 'initiateEmailChange', action)
        }
    },
    CHANGE_EMAIL: {
        initialState: Object.assign({}, defaultRequestState),
        key: 'initiateEmailChange',
        reducers: {
            request: (state) => setInitialRequestState(state, 'initiateEmailChange'),
            success: (state) => setSucceededRequestState(state, 'initiateEmailChange'),
            failed: (state, action) => setFailedRequestState(state, 'initiateEmailChange', action)
        }
    },
    SEND_VERIFICATION_EMAIL: {
        initialState: Object.assign({}, defaultRequestState),
        key: 'sendVerificationEmail',
        reducers: {
            request: (state) => setInitialRequestState(state, 'sendVerificationEmail'),
            success: (state) => setSucceededRequestState(state, 'sendVerificationEmail'),
            failed: (state, action) => setFailedRequestState(state, 'sendVerificationEmail', action)
        }
    }
};
export const actions = Object.keys(stores).reduce((acc, cur) => {
    acc[cur + '_REQUESTED'] = namespace + '/' + cur + '_REQUESTED';
    acc[cur + '_SUCCEEDED'] = namespace + '/' + cur + '_SUCCEEDED';
    acc[cur + '_FAILED'] = namespace + '/' + cur + '_FAILED';
    return acc;
}, {});

const { initialApiState, handlers } = Object.keys(stores).reduce((acc, cur) => {
    acc.initialApiState[stores[cur].key] = stores[cur].initialState;
    acc.handlers[namespace + '/' + cur + '_REQUESTED'] = namespace + '/' + cur + '_REQUESTED';
    acc.handlers[namespace + '/' + cur + '_SUCCEEDED'] = namespace + '/' + cur + '_SUCCEEDED';
    acc.handlers[namespace + '/' + cur + '_FAILED'] = namespace + '/' + cur + '_FAILED';
    return acc;
}, { handlers: [], initialApiState: {} });

export const reducer = createReducer({
    profile: null,
    ...initialApiState
}, handlers);


export const fetchProfile = (force = false) => ({
    types: [actions.FETCH_PROFILE_REQUESTED, actions.FETCH_PROFILE_SUCCEEDED, actions.FETCH_PROFILE_FAILED],
    shouldCallAPI: state => force || !state.profile.profile,
    callAPI: async () => await post(endpoint)
});

export const initiateEmailChange = (email, password) => ({
    types: [actions.INITIATE_CHANGE_EMAIL_REQUESTED, actions.INITIATE_CHANGE_EMAIL_SUCCEEDED, actions.INITIATE_CHANGE_EMAIL_FAILED],
    callAPI: async () => await post(endpoint + 'initiate-email-change', { email, password }),
    payload: { email }
});

export const changeEmail = (email, code) => ({
    types: [actions.CHANGE_EMAIL_REQUESTED, actions.CHANGE_EMAIL_SUCCEEDED, actions.CHANGE_EMAIL_FAILED],
    callAPI: async () => await post(endpoint + 'change-email', { email, code }),
    payload: { email }
});

export const sendVerificationEmail = () => ({
    types: [actions.SEND_VERIFICATION_EMAIL_REQUESTED, actions.SEND_VERIFICATION_EMAIL_SUCCEEDED, actions.SEND_VERIFICATION_EMAIL_FAILED],
    callAPI: async () => await post(endpoint + 'send-verification-email')
});

export default {
    fetchProfile,
    initiateEmailChange,
    changeEmail,
    sendVerificationEmail
};
