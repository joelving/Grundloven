export const isEmail = (email) => { return /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/.test(email); };
export const isNullOrWhitespace = (input) => { return input === null || input === undefined || /^\s*$/.test(input); };

export default {
    isEmail,
    isNullOrWhitespace
};
