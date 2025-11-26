const dbName = 'TtsFixerDb';
const storeName = 'customRules';
const dbVersion = 1;

function openDb() {
  return new Promise((resolve, reject) => {
    const req = indexedDB.open(dbName, dbVersion);
    req.onupgradeneeded = (event) => {
      const db = event.target.result;
      if (!db.objectStoreNames.contains(storeName)) {
        const store = db.createObjectStore(storeName, { keyPath: 'Identifier' });
        store.createIndex('Name', 'Name', { unique: false });
        store.createIndex('Prioity', 'Prioity', { unique: false });
        store.createIndex('IsEnabled', 'IsEnabled', { unique: false });
        store.createIndex('UseRegex', 'UseRegex', { unique: false });
        store.createIndex('CaseSensetive', 'CaseSensetive', { unique: false });
        store.createIndex('Pattern', 'Pattern', { unique: false });
        store.createIndex('Replacement', 'Replacement', { unique: false });
        store.createIndex('CreatedAt', 'CreatedAt', { unique: false });
      }
    };
    req.onsuccess = () => resolve(req.result);
    req.onerror = () => reject(req.error);
  });
}

export async function getAll() {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(storeName, 'readonly');
    const store = tx.objectStore(storeName);
    const req = store.getAll();
    req.onsuccess = () => resolve(req.result ?? []);
    req.onerror = () => reject(req.error);
  });
}

export async function add(rule) {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(storeName, 'readwrite');
    const store = tx.objectStore(storeName);
    const req = store.add(rule);
    req.onsuccess = () => resolve(true);
    req.onerror = () => reject(req.error);
  });
}

export async function update(rule) {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(storeName, 'readwrite');
    const store = tx.objectStore(storeName);
    const req = store.put(rule);
    req.onsuccess = () => resolve(true);
    req.onerror = () => reject(req.error);
  });
}

export async function remove(id) {
  const db = await openDb();
  return new Promise((resolve, reject) => {
    const tx = db.transaction(storeName, 'readwrite');
    const store = tx.objectStore(storeName);
    const req = store.delete(id);
    req.onsuccess = () => resolve(true);
    req.onerror = () => reject(req.error);
  });
}
