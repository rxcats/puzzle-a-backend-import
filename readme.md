# Puzzle A Backend

## 소개

- Firebase, Redis, Mysql 을 사용하여 퍼즐게임 백엔드를 지원하기 위한 프로젝트

### 프로젝트 구성

- GameApi: API
- GameExtensions: 확장함수 모음
- GameRedis: Redis 를 이용한 유저 세션 관리
- GameRepository: EntityFramework 를 이용한 게임 데이터 관리
- Tests: 모듈 테스트

### API

- Auth/SignIn : 로그인
- LeaderBoard/SaveScore : 리더보드 점수 저장
- LeaderBoard/TopScores : 리더보드 점수 조회

### dotnet project 관리

-`설치` <https://dotnet.microsoft.com/download>

-`csproj 추가`

```shell
dotnet new classlib -o ProjectName
```

- `솔루션 에 csproj 추가`

```shell
dotnet sln add ProjectName\ProjectName.csproj
```

DB 스키마: [Docs/db/puzzle_a_schema.sql](Docs/db/puzzle_a_schema.sql)

### WSL Ubuntu 18.04

- `수동설치` <https://docs.microsoft.com/ko-kr/windows/wsl/install-manual>
- `스토어 링크` <https://www.microsoft.com/ko-kr/p/ubuntu-1804-lts/9n9tngvndl3q?rtc=1&activetab=pivot:overviewtab>

### Ubuntu Repository 변경

- 카카오 Mirror 설정

```shell
sudo sed -i -re 's/([a-z]{2}.)?archive.ubuntu.com|security.ubuntu.com/mirror.kakao.com/g' /etc/apt/sources.list
sudo apt update && sudo apt upgrade
```

### Percona MySql 5.7

- `Install`

```shell
sudo apt install -y gnupg2
wget https://repo.percona.com/apt/percona-release_latest.$(lsb_release -sc)_all.deb
sudo dpkg -i percona-release_latest.$(lsb_release -sc)_all.deb
sudo apt update
sudo apt install -y percona-server-server-5.7
```

- `/etc/mysql/my.cnf`

```
[mysqld]
character-set-server = utf8mb4
collation-server = utf8mb4_unicode_ci

```

- `서비스 시작`

```shell
sudo service mysql start
```

- `서비스 종료`

```shell
sudo service mysql stop
```

### Redis

- `Install`

```shell
sudo apt install -y redis-server
```

* `서비스 시작`

```shell
sudo service redis-server start
```

* `서비스 종료`

```shell
sudo service redis-server stop
```

### Ubuntu sudoers 추가

- `/etc/sudoers`
- sudo 명령어 실행시 비밀번호 입력 생략 가능하도록 설정

```
# Allow members of group sudo to execute any command
%sudo ALL=NOPASSWD: /usr/sbin/service
```

### Windows 시작 스크립트

- `%homepath%\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup\start_service.cmd`

```shell
bash -c "sudo service mysql start"
bash -c "sudo service redis-server start"
```