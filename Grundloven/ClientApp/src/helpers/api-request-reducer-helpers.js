export const defaultRequestState = {
    isLoading: false,
    succeded: false,
    problemDetails: null
};

export const setInitialRequestState = (state, key) => {
    let newState = { ...state };
    newState[key] = {
        isLoading: true,
        succeded: false,
        problemDetails: null
    };
    return newState;
};

export const setSucceededRequestState = (state, key) => {
    let newState = { ...state };
    newState[key] = {
        isLoading: false,
        succeded: true,
        problemDetails: null
    };
    return newState;
};

export const setFailedRequestState = (state, key, action) => {
    let newState = { ...state };
    newState[key] = {
        isLoading: false,
        succeded: false,
        problemDetails: action.problemDetails
    };
    return newState;
};