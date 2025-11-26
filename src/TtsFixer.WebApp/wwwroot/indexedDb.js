const dbName = 'TtsFixerDb';
const storeName = 'customRules';
const dbVersion = 2;

function openDb() {
  return new Promise((resolve, reject) => {
    const req = indexedDB.open(dbName, dbVersion);
    req.onupgradeneeded = (event) => {
      const db = event.target.result;
      const oldVersion = event.oldVersion;
      
      // Delete old store if it exists (migration from v1)
      if (oldVersion < 2 && db.objectStoreNames.contains(storeName)) {
        db.deleteObjectStore(storeName);
      }
      
      // Create new store with correct schema
      if (!db.objectStoreNames.contains(storeName)) {
        const store = db.createObjectStore(storeName, { keyPath: 'identifier' });
        store.createIndex('Name', 'name', { unique: false });
        store.createIndex('Prioity', 'prioity', { unique: false });
        store.createIndex('IsEnabled', 'isEnabled', { unique: false });
        store.createIndex('UseRegex', 'useRegex', { unique: false });
        store.createIndex('CaseSensetive', 'caseSensetive', { unique: false });
        store.createIndex('Pattern', 'pattern', { unique: false });
        store.createIndex('Replacement', 'replacement', { unique: false });
        store.createIndex('CreatedAt', 'createdAt', { unique: false });
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
