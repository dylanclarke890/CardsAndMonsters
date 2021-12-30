function setItem(key, object) {
    localStorage.setItem(key, object);
}

function getItem(key) {
    var result = localStorage.getItem(key);
    return result;
}

function deleteItem(key) {
    localStorage.removeItem(key);
}

