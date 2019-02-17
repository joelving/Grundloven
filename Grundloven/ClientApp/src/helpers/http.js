import auth from './auth';

export async function post(url = ``, data = null, json = true) {
    let headers = {
        "Content-Type": json ? "application/json" : "application/x-www-form-urlencoded;charset=UTF-8",
    };
    const token = auth.getToken();
    if (token !== null)
        headers.Authorization = token.token_type + ' ' + token.access_token;

    let postData = {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        credentials: "same-origin",
        headers,
        redirect: "follow",
        referrer: "no-referrer"
    };
    
    if (data !== null) {
        let body = null;
        if (json) {
            body = JSON.stringify(data)
        }
        else {
            body = new URLSearchParams();
            for (let key in data)
            body.append(key, data[key]);
        }
        postData.body = body;
    }

    let response = null;
    try {
        response = await fetch(url, postData);
    }
    catch (err) {
        throw {
            title: "Netværksfejl",
            detail: "Der opstod et problem med at forbinde til serveren. Prøv igen senere."
        };
    }
    let responseData = null;
    try {
        responseData = await response.json();
    }
    catch (err) {
        throw {
            title: "Serverfejl",
            detail: "Serveren svarede på en uventet måde. Prøv igen senere."
        };
    }
    if (!response.ok) {
        throw responseData;
    }
    return responseData;
}
