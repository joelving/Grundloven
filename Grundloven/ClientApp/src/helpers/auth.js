const tokenPrefix = 'security.bearer';

function storageAvailable(type) {
    try {
        var storage = window[type],
            x = '__storage_test__';
        storage.setItem(x, x);
        storage.removeItem(x);
        return true;
    }
    catch(e) {
        return e instanceof DOMException && (
            // everything except Firefox
            e.code === 22 ||
            // Firefox
            e.code === 1014 ||
            // test name field too, because code might not be present
            // everything except Firefox
            e.name === 'QuotaExceededError' ||
            // Firefox
            e.name === 'NS_ERROR_DOM_QUOTA_REACHED') &&
            // acknowledge QuotaExceededError only if there's something already stored
            storage.length !== 0;
    }
}

const inMemoryStorage = {
    _data: {},
    setItem: (id, val) => this._data[id] = String(val),
    getItem: id => this._data.hasOwnProperty(id) ? this._data[id] : null,
    removeItem: id => delete this._data[id],
    clear: () => this._data = {}
};
const storage = storageAvailable('localStorage') ? window.localStorage : inMemoryStorage;

export default {
    setToken: ({ token_type, access_token, expires_in}) => {
        storage.setItem(tokenPrefix + '.token_type', token_type);
        storage.setItem(tokenPrefix + '.access_token', access_token);
        storage.setItem(tokenPrefix + '.expires', Date.now() + (expires_in * 1000));
    },
    getToken: () => {
        let expiration = storage.getItem(tokenPrefix + '.expires');
        if (!expiration) return null;

        let now = Date.now();
        if (now > expiration) {
            this.removeToken();
            return null;
        }

        return {
            token_type: storage.getItem(tokenPrefix + '.token_type'),
            access_token: storage.getItem(tokenPrefix + '.access_token'),
            expires_in: (expiration - now) / 1000
        };
    },
    removeToken: () => {
        storage.removeItem(tokenPrefix + '.token_type');
        storage.removeItem(tokenPrefix + '.access_token');
        storage.removeItem(tokenPrefix + '.expires');
    }
};
