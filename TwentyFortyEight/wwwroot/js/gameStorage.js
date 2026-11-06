const databaseName = "TwentyFortyEight";
const storeName = "gameData";
const databaseVersion = 1;

let databasePromise;

function openDatabase() {
    if (databasePromise)
        return databasePromise;

    databasePromise = new Promise((resolve, reject) => {
        const request = indexedDB.open(databaseName, databaseVersion);

        request.onupgradeneeded = () => {
            const database = request.result;

            if (!database.objectStoreNames.contains(storeName)) {
                database.createObjectStore(storeName);
            }
        };

        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error);
    });

    return databasePromise;
}

export async function load() {
    const database = await openDatabase();

    return new Promise((resolve, reject) => {
        const transaction = database.transaction(storeName, "readonly");
        const request = transaction.objectStore(storeName).get("main");

        request.onsuccess = () => resolve(request.result ?? null);
        request.onerror = () => reject(request.error);
    });
}

export async function save(data) {
    const database = await openDatabase();

    return new Promise((resolve, reject) => {
        const transaction = database.transaction(storeName, "readwrite");

        transaction.objectStore(storeName).put(data, "main");

        transaction.oncomplete = () => resolve();
        transaction.onerror = () => reject(transaction.error);
        transaction.onabort = () => reject(transaction.error);
    });
}