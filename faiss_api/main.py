from typing import List
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
import numpy as np
import faiss

app = FastAPI()

dim = 1536  # Example dimension, match your embedding size
index = faiss.IndexFlatL2(dim)
vectors = []
metadatas = []

class VectorAddRequest(BaseModel):
    vector: list[float]
    metadata: dict

class VectorSearchRequest(BaseModel):
    query: list[float]
    top_k: int = 5

class BatchAddRequest(BaseModel):
    fileName: str
    embeddings: List[List[float]]
    texts: List[str]

@app.post("/add")
def add_vectors(req: BatchAddRequest):
    if len(req.embeddings) != len(req.texts):
        raise HTTPException(status_code=400, detail="Embeddings and texts must have the same length")
    arr = np.array(req.embeddings, dtype=np.float32)
    if arr.shape[1] != dim:
        raise HTTPException(status_code=400, detail=f"Each embedding must have dimension {dim}")
    index.add(arr)
    vectors.extend(req.embeddings)
    for text in req.texts:
        metadatas.append({"text": text, "fileName": req.fileName})
    return {"status": "ok", "count": index.ntotal}

@app.post("/search")
def search_vector(req: VectorSearchRequest):
    q = np.array([req.query], dtype=np.float32)
    if q.shape[1] != dim:
        raise HTTPException(status_code=400, detail=f"Query must have dimension {dim}")
    D, I = index.search(q, req.top_k)
    results = []
    for idx, dist in zip(I[0], D[0]):
        if idx == -1:
            continue
        results.append({
            "vector": vectors[idx],
            "metadata": metadatas[idx],
            "distance": float(dist)
        })
    return {"results": results}

# To run: uvicorn main:app --host 0.0.0.0 --port 8000