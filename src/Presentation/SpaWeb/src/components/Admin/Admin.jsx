import React, { useState, useEffect, Fragment } from "react";
import adminService from "../../services/adminService";
import { Spinner, Table, Form } from "react-bootstrap";
import ReactPaginate from "react-paginate";
import { toast } from "react-toastify";

export const Admin = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [paginationQuery, setPaginationQuery] = useState({
    pageNumber: 1,
    pageSize: 7,
  });
  const [users, setUsers] = useState([]);
  const [totalPages, setTotalPages] = useState();

  useEffect(() => {
    adminService.getUsers(paginationQuery).then((response) => {
      setUsers(response.data.data);
      setTotalPages(response.data.totalPages);
      setIsLoading(false);
    });
  }, [paginationQuery]);

  const handlePageClick = (data) => {
    let pageNumber = data.selected + 1;
    setPaginationQuery((prev) => ({ ...prev, pageNumber }));
  };

  const handleAddUserToRole = (event, index) => {
    const role = event.target.value;
    const email = users[index].email;

    adminService.addToRole(email, role).then(() => {
      setUsers((prev) => [
        ...prev.map((user, currentIndex) => {
          if (currentIndex === index) {
            return {
              ...user,
              currentRoles: [...user.currentRoles, role],
              nonCurrentRoles: user.nonCurrentRoles.filter(
                (nonCurrentRole) => nonCurrentRole !== role
              ),
            };
          }

          return user;
        }),
      ]);
      toast.success(`${email} has been successfully promoted to ${role}`);
    });
  };

  const handleRemoveUserFromRole = (event, index) => {
    const role = event.target.value;
    const email = users[index].email;

    adminService.removeFromRole(email, role).then(() => {
      setUsers((prev) => [
        ...prev.map((user, currentIndex) => {
          if (currentIndex === index) {
            return {
              ...user,
              currentRoles: user.currentRoles.filter(
                (currentRole) => currentRole !== role
              ),

              nonCurrentRoles: [...user.nonCurrentRoles, role],
            };
          }

          return user;
        }),
      ]);

      toast.success(`${email} has been successfully demoted from ${role}`);
    });
  };

  return isLoading ? (
    <Spinner animation="border" />
  ) : (
    <Fragment>
      <Table responsive striped bordered hover size="sm">
        <thead>
          <tr>
            <th>Email</th>
            <th>Roles</th>
            <th>Add to role</th>
            <th>Remove from role</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user, index) => {
            return (
              <tr key={index}>
                <td>{user.email}</td>
                <td>
                  {user.currentRoles.length !== 0 ? user.currentRoles : "User"}
                </td>
                <td>
                  <Form.Group>
                    <Form.Control
                      as="select"
                      custom
                      value="Select role"
                      onChange={(e) => handleAddUserToRole(e, index)}
                    >
                      <option disabled>Select role</option>
                      {user.nonCurrentRoles.map((role, index) => {
                        return <option key={index}>{role}</option>;
                      })}
                    </Form.Control>
                  </Form.Group>
                </td>
                <td>
                  <Form.Group>
                    <Form.Control
                      as="select"
                      custom
                      value="Select role"
                      onChange={(e) => handleRemoveUserFromRole(e, index)}
                    >
                      <option disabled>Select role</option>
                      {user.currentRoles.map((role, index) => {
                        return <option key={index}>{role}</option>;
                      })}
                    </Form.Control>
                  </Form.Group>
                </td>
              </tr>
            );
          })}
        </tbody>
      </Table>
      <ReactPaginate
        previousLabel={"previous"}
        nextLabel={"next"}
        breakLabel={"..."}
        pageCount={totalPages}
        marginPagesDisplayed={3}
        pageRangeDisplayed={5}
        onPageChange={handlePageClick}
        breakClassName={"page-item"}
        breakLinkClassName={"page-link"}
        containerClassName={"pagination justify-content-end"}
        pageClassName={"page-item"}
        pageLinkClassName={"page-link"}
        previousClassName={"page-item"}
        previousLinkClassName={"page-link"}
        nextClassName={"page-item"}
        nextLinkClassName={"page-link"}
        activeClassName={"active"}
      />
    </Fragment>
  );
};
