from sentence_transformers import SentenceTransformer
from numpy import dot
from math import sqrt
import json

def get_tweets():
    tweets = None
    return tweets

def sort_by_sim(query_embedding,document_embeddings,documents):
    return 0
    
def glove_top25(query,documents):
    return []

def minilm_top25(query,documents):
    return []
        
## Test Code

tweets = get_tweets()

print("**************GLOVE*****************")
for p in glove_top25("I am looking for a job.",tweets): print(p)

print("**************MINILM*****************")
for p in minilm_top25("I am looking for a job.",tweets): print(p)