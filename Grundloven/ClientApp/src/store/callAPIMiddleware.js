export default function callAPIMiddleware({ dispatch, getState }) {
    return next => async action => {
        const { types, callAPI, shouldCallAPI = () => true, payload = {} } = action;

        if (!types) {
            // Normal action: pass it on
            return next(action);
        }

        if (!Array.isArray(types) || types.length !== 3 || !types.every(type => typeof type === 'string')) {
            throw new Error('Expected an array of three string types.');
        }

        if (typeof callAPI !== 'function') {
            throw new Error('Expected callAPI to be a function.');
        }

        if (!shouldCallAPI(getState())) {
            return;
        }

        const [requestType, successType, failureType] = types;

        dispatch({ ...payload, type: requestType });

        try {
            var response = await callAPI();
            dispatch({ ...payload, ...response, type: successType });
        }
        catch (problemDetails) {
            dispatch({ ...payload, problemDetails, type: failureType });
        }
    }
};
