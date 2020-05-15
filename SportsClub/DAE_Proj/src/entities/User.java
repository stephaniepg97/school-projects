package entities;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import javax.validation.constraints.Pattern;

import java.io.Serializable;
import java.math.BigInteger;
import java.nio.ByteBuffer;
import java.nio.CharBuffer;
import java.nio.charset.Charset;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.*;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.stream.Collectors;
import javax.persistence.Entity;


@Entity
@Table(name="USERS")
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@MappedSuperclass
@NamedQueries({
        @NamedQuery(
                name = "user.all",
                query = "SELECT u FROM User u"
        )
})
public class User implements entities.Entity {

    @Id
    protected String username;

    @NotNull
    protected String password;

    @NotNull
   // @Pattern(regexp = "[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", message = "{invalid.email}")
    protected String email;

    @NotNull
    protected String name;

    @Version
    protected int version;

    @OneToMany(mappedBy = "user", fetch = FetchType.LAZY)
    protected Set<Email> emails;

    public User() {
        this.emails = new LinkedHashSet<>();
    }

    public User(String username, String password, String name, String email) {
        this.username = username;
        this.password = hashPassword(password);
        this.email = email;
        this.name = name;
        this.emails = new LinkedHashSet<>();
    }

    public static String hashPassword(String password) {
        char[] encoded = null;
        try {
            ByteBuffer buffer = Charset.defaultCharset().encode(CharBuffer.wrap(password));
            byte[] bytes = buffer.array();
            MessageDigest mdEnc = MessageDigest.getInstance("SHA-256");
            mdEnc.update(bytes, 0, password.toCharArray().length);
            encoded = new BigInteger(1, mdEnc.digest()).toString(16).toCharArray();
        } catch (NoSuchAlgorithmException ex) {
            Logger.getLogger(User.class.getName()).log(Level.SEVERE, null, ex);
        }
        return new String(encoded);
    }

    public void add(Email e) {
        this.emails.add(e);
    }

    public void remove(Email e) { this.emails.remove(e); }

    public void removeEmail(int id) {
        List<Email> result = this.emails.stream().filter(e -> e.getId() == id).collect(Collectors.toList());
        if (result.size() == 1) remove(result.get(0));
    }

    public int getVersion() {
        return version;
    }

    public void setVersion(int version) {
        this.version = version;
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

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setEmails(Collection<Email> payments) {
        this.emails = payments.stream().collect(Collectors.toSet());
    }

    public Set<Email> getEmails() {
        return emails;
    }

    public void setEmails(Set<Email> emails) {
        this.emails = emails;
    }

    @Override
    public boolean equals(Object obj) {
        return obj instanceof User && ((User) obj).username.equals(username);
    }

    @Override
    public int hashCode() {
        int hash = 7;
        hash = 79 * hash + Objects.hashCode(this.username);
        return hash;
    }
}
