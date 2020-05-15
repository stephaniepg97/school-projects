package dtos;

import java.util.Objects;

import static entities.User.hashPassword;

public class AuthDTO implements DTO {

    private String username;
    private String password;

    public AuthDTO() {
    }

    public AuthDTO (String username, String password) {
        this.username = username;
        this.password = password;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    @Override
    public void reset() {
        this.username = this.password = null;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        AuthDTO authDTO = (AuthDTO) o;
        return username.equals(authDTO.username);
    }

    @Override
    public int hashCode() {
        return Objects.hash(username);
    }
}
