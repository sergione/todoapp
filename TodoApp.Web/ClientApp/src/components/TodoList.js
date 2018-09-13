import React, {Component } from 'react';
import { Query } from "react-apollo";
import gql from "graphql-tag";
import {Checkbox} from "react-bootstrap";

const TodoList = () => (
    <Query
        query={gql`
          {
            todos {
              id
              description
              complete
            }
          }
        `}
    >
        {({ loading, error, data }) => {
            if (loading) return <p>Loading...</p>;
            if (error) return <p>Error :(</p>;

            return data.todos.map(({ id, description, complete }) => (
                <div key={id}>
                    <p><Checkbox checked={complete} />{description}</p>
                </div>
            ));
        }}
    </Query>
);

export default TodoList;