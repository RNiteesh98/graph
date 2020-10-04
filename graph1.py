class Graph:
    def __init__(self):
        self.graph={}
        
    def addEdge(self,u,v):
        if u in self.graph:
            self.graph[u].append(v)
        else:
            self.graph[u]=[v]
    
    def printList(self):
        for i in self.graph:
            print(i, self.graph[i])
            
    ######################
    
    def BFS(self,s):
        visited = [False]*(len(self.graph))
        
        queue=[]
        queue.append(s)
        visited[s]=True
        
        while queue:
            var = queue.pop(0)
            print(var,end=" ")
            
            for i in self.graph[var]:
                if visited[i]==False:
                    queue.append(i)
                    visited[i]=True
                    
    ##########################
    
    def DFS(self,s):
        visited=[False]*(len(self.graph))
        
        stack=[]
        stack.append(s)
        visited[s]=True
        
        while stack:
            var=stack.pop(-1)
            print(var, end=" ")
            
            for i in self.graph[var]:
                if visited[i]==False:
                    stack.append(i)
                    visited[i]=True
    
    ############ BFS for non non connected graphs
    
    def helper_BFS(self,i,visited):
        visited[i]=True
        queue=[]
        queue.append(i)
        
        while queue:
            var = queue.pop(0)
            print(var,end=" ")
            
            for i in self.graph[var]:
                if(visited[i]==False):
                    queue.append(i)
                    visited[i]=True
    
    def allBFS(self):
        visited=[False]*(len(self.graph))
        
        for i in range(len(visited)):
            if visited[i]==False:
                self.helper_BFS(i,visited)
                
    #####################   Has Path
    
    def hasPath(self,u,v):
        visited=[False]*(len(self.graph))
        queue=[]
        queue.append(u)
        visited[u]=True
        
        while queue:
            var = queue.pop(0)
            if(var==v):
                return True
            for i in self.graph[var]:
                if(visited[i]==False):
                    queue.append(i)
                    visited[i]=True
            
        return False        
        
    ################# Get Path using DFS
    
    def getPath(self,u,v):
        visited=[False]*(len(self.graph))
        stack=[]
        stack.append(u)
        visited[u]=True
        path=[]
        while stack:
            var=stack.pop(-1)
            path.append(var)
            if(var==v):
                return path
            for i in self.graph[var]:
                if visited[i]==False:
                    stack.append(i)
                    visited[i]=True
        return False
        
    ###################### Get path using BFS
    
    def printGetPath(self,intmap,u,v):
        var=v
        print(v, end=" ")
        while var!=u:
            print(intmap[var],end=" ")
            var=intmap[var]
    
    def getPathBFS(self,u,v):
        visited=[False]*(len(self.graph))
        queue=[]
        queue.append(u)
        visited[u]=True
        intmap={}
        while queue:
            var=queue.pop(0)
            if(var==v):
                self.printGetPath(intmap,u,v)
                # return "Yes Path"
            for i in self.graph[var]:
                if visited[i]==False:
                    queue.append(i)
                    visited[i]=True
                    # if i in intmap:
                    #     pass
                    # else:
                    intmap[i]=var
        return "No Path"
    ##############################
    
     
    
            
    
g=Graph()           
g.addEdge(0,1)
g.addEdge(0,2)
g.addEdge(1,2)
g.addEdge(2,0)
g.addEdge(2,3)
g.addEdge(3,3)

# g.addEdge(4,4)

# g.printList()

# g.BFS(2)
# print()
# g.DFS(2)

# g.allBFS()

# print(g.hasPath(3,4))
# print(g.getPathBFS(0,3))