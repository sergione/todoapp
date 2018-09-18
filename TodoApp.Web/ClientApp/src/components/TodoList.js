import React from 'react';
import { Query } from "react-apollo";
import gql from "graphql-tag";
import TodoListItem from "./TodoListItem";

const GET_TODOS = gql`
    query getTodos($offset: Int, $limit: Int)
      {
          todos(offset: $offset, limit: $limit) {
              id
              description
              complete
          }
      }
    `;

const TodoListInternal = ({todos}) => {
    return todos.map(({ id, description, complete }) => (
            <TodoListItem key={id} id={id} checked={complete} description={description}/>
        ));    
};

const TodoList = () => (
    <Query 
        query={GET_TODOS}
        variables={{
            offset: 0,
            limit: 2
        }}
        fetchPolicy="cache-and-network"
    >
        {({ loading, error, data, fetchMore }) => {
            if (loading) return <p>Loading...</p>;
            if (error) return <p>Error :(</p>;
            const onLoadMore = () => {
                fetchMore({
                    variables: {offset: data.todos.length},
                    updateQuery: (prev, {fetchMoreResult}) => {
                        if (!fetchMoreResult) return prev;
                        return Object.assign({}, prev, {
                            todos: [...prev.todos, ...fetchMoreResult.todos]
                        });
                    }
                })    
            };
            return <div>
                <TodoListInternal todos={data.todos}/>
                <button onClick={onLoadMore}>load more</button>
            </div>
        }}
    </Query>
);

export default TodoList;